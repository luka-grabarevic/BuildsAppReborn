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
        #region Constructors

        [ImportingConstructor]
        public BuildMonitor(LazyContainer<IBuildProvider, IBuildProviderMetadata> buildProviders, [ImportMany] IEnumerable<INotificationProvider> notificationProviders)
        {
            this.buildProviders = buildProviders;
            this.notificationProvider = notificationProviders.FirstOrDefault();
            this.timer.Elapsed += (sender, args) => BeginPollingBuilds();
        }

        #endregion

        #region Implementation of IBuildMonitorAdvanced

        public void Start(IEnumerable<BuildMonitorSettings> settings, TimeSpan pollingInterval)
        {
            Stop();

            Initialize(settings);

            if (this.providerSettingsGroup.Any())
            {
                this.timer.Interval = pollingInterval.TotalMilliseconds;
                this.timer.Start();
            }

            OnMonitorStarted();
        }

        public void Stop()
        {
            this.timer.Stop();
            this.providerSettingsGroup.Clear();

            OnMonitorStopped();
        }

        public event BuildsUpdatedEventHandler BuildsUpdated;

        public void BeginPollingBuilds()
        {
            if (this.isPolling) return;

            this.isPolling = true;
            foreach (var pair in this.providerSettingsGroup)
            {
                foreach (var setting in pair.Value)
                {
                    PollBuilds(pair.Key, setting);
                }
            }
            this.isPolling = false;
        }

        public Boolean IsConfigured => this.providerSettingsGroup.Any() && this.providerSettingsGroup.SelectMany(a => a.Value).SelectMany(a => a.SelectedBuildDefinitions).Any();

        public event EventHandler MonitorStopped;

        public event EventHandler MonitorStarted;

        #endregion

        #region Private Methods

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

        private async void PollBuilds(IBuildProvider provider, BuildMonitorSettings settings)
        {
            try
            {
                var builds = await Task.Run(() => provider.GetBuilds(settings.SelectedBuildDefinitions, settings));
                OnBuildsUpdated(builds.ToList());
            }
            catch (Exception exception)
            {
                this.logger.Warn("Failure on polling builds", exception);
                this.notificationProvider.ShowMessage("Failure on getting builds", $"Please check the connection for project(s) {String.Join(", ", settings.SelectedBuildDefinitions.Select(b => b.Project.Name).Distinct())}.");
            }
        }

        #endregion

        #region Private Fields

        private readonly LazyContainer<IBuildProvider, IBuildProviderMetadata> buildProviders;

        private Boolean isPolling;

        private ILog logger = LogManager.GetLogger(typeof(BuildMonitor));
        private INotificationProvider notificationProvider;

        private readonly Dictionary<IBuildProvider, ICollection<BuildMonitorSettings>> providerSettingsGroup = new Dictionary<IBuildProvider, ICollection<BuildMonitorSettings>>();

        private readonly Timer timer = new Timer();

        #endregion
    }
}