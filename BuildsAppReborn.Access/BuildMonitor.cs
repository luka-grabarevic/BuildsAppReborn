using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;
using BuildsAppReborn.Infrastructure;
using log4net;

namespace BuildsAppReborn.Access
{
    [InheritedExport(typeof(IBuildMonitorAdvanced))]
    [Export(typeof(IBuildMonitorBasic))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class BuildMonitor : IBuildMonitorAdvanced
    {
        [ImportingConstructor]
        public BuildMonitor(LazyContainer<IBuildProvider, IBuildProviderMetadata> buildProviders,
                            LazyContainer<INotificationProvider, IPriorityMetadata> notificationProviders,
                            IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer,
                            GeneralSettings generalSettings)
        {
            this.buildProviders = buildProviders;
            this.buildDefinitionEqualityComparer = buildDefinitionEqualityComparer;
            this.generalSettings = generalSettings;
            this.notificationProvider = notificationProviders.GetSupportedNotificationProvider();
            this.timer.Elapsed += (sender, args) => BeginPollingBuildsAsync().GetAwaiter().GetResult();
            this.timer.ProgressElapsed += (sender, args) =>
            {
                OnProgressUpdated(MaximumProgress - this.timer.CurrentInterval);
            };
        }

        public Boolean IsConfigured => this.providerSettingsGroup.Any() && (this.generalSettings.ViewStyle == BuildViewStyle.GroupByPullRequest ||
                                                                            this.providerSettingsGroup.SelectMany(a => a.Value).SelectMany(a => a.SelectedBuildDefinitions).Any());

        public Double MaximumProgress => this.generalSettings.PollingInterval.TotalMilliseconds;

        public async Task BeginPollingBuildsAsync()
        {
            var builds = new List<IBuild>();
            foreach (var pair in this.providerSettingsGroup)
            {
                foreach (var setting in pair.Value)
                {
                    builds.AddRange(await PollBuildsAsync(pair.Key, setting).ConfigureAwait(false));
                }
            }

            OnBuildsUpdated(builds);
        }

        public void Start(IEnumerable<BuildMonitorSettings> settings, GeneralSettings generalSettingsParam)
        {
            Stop();

            Initialize(settings);

            this.generalSettings = generalSettingsParam;

            if (this.providerSettingsGroup.Any())
            {
                this.timer.Interval = this.generalSettings.PollingInterval.TotalMilliseconds;
                this.timer.Start();
            }

            OnMonitorStarted();
        }

        public void Stop()
        {
            this.timer.Stop();
            this.providerSettingsGroup.Clear();
            this.generalSettings = null;

            OnMonitorStopped();
        }

        public event BuildsUpdatedEventHandler BuildsUpdated;

        public event EventHandler MonitorStarted;

        public event EventHandler MonitorStopped;

        public event PollingProgressUpdated ProgressUpdated;

        private void Initialize(IEnumerable<BuildMonitorSettings> settings)
        {
            var groupedByProviderId = settings.GroupBy(a => a.BuildProviderId);
            foreach (var grp in groupedByProviderId)
            {
                var provider = this.buildProviders.GetSingleOrDefault(a => a.Id == grp.Key);
                if (provider != null)
                {
                    this.providerSettingsGroup.Add(provider, grp.ToList());
                }
            }
        }

        private void OnBuildsUpdated(ICollection<IBuild> builds)
        {
            BuildsUpdated?.Invoke(builds);
        }

        private void OnMonitorStarted()
        {
            MonitorStarted?.Invoke(this, EventArgs.Empty);
        }

        private void OnMonitorStopped()
        {
            MonitorStopped?.Invoke(this, EventArgs.Empty);
        }

        private void OnProgressUpdated(Double progress)
        {
            ProgressUpdated?.Invoke(progress);
        }

        private async Task<IEnumerable<IBuild>> PollBuildsAsync(IBuildProvider provider, BuildMonitorSettings settings)
        {
            try
            {
                if (this.generalSettings?.ViewStyle == BuildViewStyle.GroupByPullRequest)
                {
                    var prBuilds = await provider.GetBuildsByPullRequestsAsync(settings).ConfigureAwait(false);
                    prBuilds.ThrowIfUnsuccessful();

                    var definitionsInUse = prBuilds.Data.GroupBy(a => a.Definition, build => build, this.buildDefinitionEqualityComparer).Select(a => a.Key);
                    var unusedDefinitions = settings.SelectedBuildDefinitions.Except(definitionsInUse, this.buildDefinitionEqualityComparer).ToList();
                    if (unusedDefinitions.Any())
                    {
                        var defBuilds = await provider.GetBuildsAsync(settings.SelectedBuildDefinitions, settings).ConfigureAwait(false);
                        defBuilds.ThrowIfUnsuccessful();

                        return prBuilds.Data.Concat(defBuilds.Data);
                    }

                    return prBuilds.Data;
                }

                var builds = await provider.GetBuildsAsync(settings.SelectedBuildDefinitions, settings).ConfigureAwait(false);
                builds.ThrowIfUnsuccessful();

                return builds.Data;
            }
            catch (DataResponseUnsuccessfulException ex)
            {
                this.logger.Warn($"Http status code {ex.StatusCode} returned while polling for builds!");
                this.notificationProvider?.ShowMessage("Failure on getting builds",
                                                       $"Please check the connection for project(s) '{String.Join(", ", settings.SelectedBuildDefinitions.Select(b => b.Project.Name).Distinct())}'. StatusCode was '{ex.StatusCode}'. See log for more details.");
            }
            catch (Exception exception)
            {
                this.logger.Warn("Failure on polling builds", exception);
                this.notificationProvider?.ShowMessage("Failure on getting builds",
                                                       $"Please check the connection for project(s) '{String.Join(", ", settings.SelectedBuildDefinitions.Select(b => b.Project.Name).Distinct())}'. See log for details.");
            }

            return Enumerable.Empty<IBuild>();
        }

        private readonly IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer;

        private readonly LazyContainer<IBuildProvider, IBuildProviderMetadata> buildProviders;
        private GeneralSettings generalSettings;

        private readonly ILog logger = LogManager.GetLogger(typeof(BuildMonitor));

        private readonly INotificationProvider notificationProvider;

        private readonly Dictionary<IBuildProvider, ICollection<BuildMonitorSettings>> providerSettingsGroup = new Dictionary<IBuildProvider, ICollection<BuildMonitorSettings>>();

        private readonly ProgressTimer timer = new ProgressTimer();
    }
}