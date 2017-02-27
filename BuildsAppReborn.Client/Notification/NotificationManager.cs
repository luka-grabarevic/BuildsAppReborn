using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BuildsAppReborn.Client.Resources;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;

namespace BuildsAppReborn.Client.Notification
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NotificationManager
    {
        #region Constructors

        [ImportingConstructor]
        public NotificationManager([ImportMany] IEnumerable<INotificationProvider> notificationProviders, BuildCache buildCache, IEqualityComparer<IBuild> buildEqualityComparer)
        {
            this.buildCache = buildCache;
            this.buildEqualityComparer = buildEqualityComparer;
            this.buildCache.CacheUpdated += (sender, args) => ShowNotification();

            // ToDo: find the compatible provider for this system
            this.notificationProvider = notificationProviders.FirstOrDefault();
        }

        #endregion

        #region Private Methods

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
                this.notificationProvider?.ShowBuild(build, b => IconProvider.GetCachedIconPathForBuildStatus(b.Status));
            }
        }

        #endregion

        #region Private Fields

        private readonly BuildCache buildCache;
        private readonly IEqualityComparer<IBuild> buildEqualityComparer;
        private INotificationProvider notificationProvider;
        private IList<IBuild> oldBuilds;

        #endregion
    }
}