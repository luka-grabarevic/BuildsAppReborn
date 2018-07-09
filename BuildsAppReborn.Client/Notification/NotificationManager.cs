using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.Resources;
using BuildsAppReborn.Client.ViewModels;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.Notification
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NotificationManager
    {
        [ImportingConstructor]
        public NotificationManager(LazyContainer<INotificationProvider, IPriorityMetadata> notificationProviders,
                                   BuildCache buildCache,
                                   IEqualityComparer<IBuild> buildEqualityComparer,
                                   ExportFactory<IBuildsStatusView> buildsExportFactory)
        {
            this.buildCache = buildCache;
            this.buildEqualityComparer = buildEqualityComparer;
            this.buildsExportFactory = buildsExportFactory;
            this.buildCache.CacheUpdated += (sender, args) => ShowNotification();

            this.notificationProvider = notificationProviders.GetSupportedNotificationProvider();
        }

        // Show only builds that changed since last time to prevent SPAM
        private IEnumerable<IBuild> GetChangedBuilds(IList<IBuild> builds)
        {
            var result = new List<IBuild>();

            if (this.oldBuilds != null)
            {
                foreach (var build in builds)
                {
                    var oldBuild = this.oldBuilds.SingleOrDefault(b => this.buildEqualityComparer.Equals(b, build));
                    if (oldBuild != null)
                    {
                        if (build.Status != oldBuild.Status)
                        {
                            result.Add(build);
                        }
                    }
                    else
                    {
                        result.Add(build);
                    }
                }
            }

            this.oldBuilds = builds;

            return result;
        }

        private void ShowNotification()
        {
            var builds = this.buildCache.BuildsStatus.SelectMany(a => a.AllBuildItems.Select(b => b.Build)).ToList();
            var changedBuilds = GetChangedBuilds(builds);
            foreach (var build in changedBuilds)
            {
                this.notificationProvider?.ShowBuild(build,
                                                     b => IconProvider.GetCachedIconPathForBuildStatus(b.Status),
                                                     b => { Application.Current.Dispatcher.Invoke(() => { NotifyIconViewModel.OpenWindow(this.buildsExportFactory); }); });
            }
        }

        private readonly BuildCache buildCache;
        private readonly IEqualityComparer<IBuild> buildEqualityComparer;
        private readonly ExportFactory<IBuildsStatusView> buildsExportFactory;
        private readonly INotificationProvider notificationProvider;
        private IList<IBuild> oldBuilds;
    }
}