using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using BuildsAppReborn.Client.Resources;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;
using BuildsAppReborn.Infrastructure.Collections;

namespace BuildsAppReborn.Client
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class BuildCache : ViewModelBase
    {
        #region Constructors

        [ImportingConstructor]
        public BuildCache(IBuildMonitorBasic buildMonitor, IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer)
        {
            BuildsStatus = new RangeObservableCollection<BuildStatusGroup>();
            this.buildDefinitionEqualityComparer = buildDefinitionEqualityComparer;
            buildMonitor.BuildsUpdated += OnBuildsUpdated;
            buildMonitor.MonitorStopped += (sender, args) => CacheStatus = BuildCacheStatus.NotConfigured;
            buildMonitor.MonitorStarted += (sender, args) => CacheStatus = BuildCacheStatus.Loading;

            if (buildMonitor.IsConfigured)
            {
                CacheStatus = BuildCacheStatus.Loading;
                buildMonitor.BeginPollingBuilds();
            }
            else
            {
                CacheStatus = BuildCacheStatus.NotConfigured;
            }

            UpdateCurrentIcon();
        }

        #endregion

        #region Public Properties

        public RangeObservableCollection<BuildStatusGroup> BuildsStatus { get; private set; }

        public BuildCacheStatus CacheStatus
        {
            get { return this.cacheStatus; }
            private set
            {
                this.cacheStatus = value;
                OnPropertyChanged();
            }
        }

        public String CurrentIcon
        {
            get { return this.currentIcon; }
            set
            {
                if (this.currentIcon != value)
                {
                    this.currentIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnCacheUpdated()
        {
            CacheUpdated?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Private Methods

        private async Task<Byte[]> GetRequesterImageWithDefaultCredentials(String requesterImageUrl)
        {
            return await HttpRequestHelper.GetRequestResultsAsByteArray(requesterImageUrl, CredentialCache.DefaultCredentials);
        }

        private async void OnBuildsUpdated(ICollection<IBuild> builds)
        {
            if (!builds.Any()) return;

            var buildStatusGroups = new List<BuildStatusGroup>();
            var groupByDefinition = builds.GroupBy(a => a.Definition, build => build, this.buildDefinitionEqualityComparer);
            foreach (var grp in groupByDefinition)
            {
                var newStatus = new BuildStatusGroup();

                newStatus.BuildDefinition = grp.Key;
                newStatus.AllBuildItems = grp.Select(a => new BuildItem(a)).ToList();
                newStatus.CurrentBuild.RequesterImage = await GetRequesterImageWithDefaultCredentials(newStatus.CurrentBuild.Build.Requester.ImageUrl);
                buildStatusGroups.Add(newStatus);
            }
            Application.Current.Dispatcher.Invoke(() =>
                                                  {
                                                      BuildsStatus.Clear();
                                                      BuildsStatus.AddRange(buildStatusGroups);
                                                      CacheStatus = BuildCacheStatus.Operational;
                                                  });
            UpdateCurrentIcon();
            OnCacheUpdated();
        }

        private void UpdateCurrentIcon()
        {
            if (CacheStatus == BuildCacheStatus.Loading)
            {
                CurrentIcon = IconProvider.LoadingIcon;
            }
            else if (CacheStatus == BuildCacheStatus.NotConfigured)
            {
                CurrentIcon = IconProvider.SettingsIcon;
            }
            else if (CacheStatus == BuildCacheStatus.Operational)
            {
                if (!BuildsStatus.Any()) return;

                var relevantBuilds = new List<IBuild>();
                foreach (var buildStatus in BuildsStatus)
                {
                    var lastFinishedBuild = buildStatus.AllBuildItems.Last(a => a.Build.Status != BuildStatus.Running && a.Build.Status != BuildStatus.Queued);
                    if (lastFinishedBuild != null)
                    {
                        relevantBuilds.Add(lastFinishedBuild.Build);
                    }
                }

                if (relevantBuilds.All(a => a.Status == BuildStatus.Succeeded))
                {
                    CurrentIcon = IconProvider.GetIconForBuildStatus(BuildStatus.Succeeded);
                    return;
                }

                if (relevantBuilds.Any(a => a.Status == BuildStatus.PartiallySucceeded))
                {
                    CurrentIcon = IconProvider.GetIconForBuildStatus(BuildStatus.PartiallySucceeded);
                    return;
                }

                if (relevantBuilds.Any(a => a.Status == BuildStatus.Failed))
                {
                    CurrentIcon = IconProvider.GetIconForBuildStatus(BuildStatus.Failed);
                }
            }
        }

        #endregion

        #region Private Fields

        private readonly IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer;

        private BuildCacheStatus cacheStatus;
        private String currentIcon;

        #endregion

        public event EventHandler CacheUpdated;
    }
}