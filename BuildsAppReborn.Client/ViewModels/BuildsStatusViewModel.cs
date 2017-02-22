using System;
using System.ComponentModel.Composition;
using System.Timers;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BuildsStatusViewModel : ViewModelBase, ICloseable
    {
        #region Constructors

        [ImportingConstructor]
        public BuildsStatusViewModel(BuildCache buildCache)
        {
            BuildCache = buildCache;
            this.timer = new Timer {Interval = 10000, AutoReset = true}; // update every 10 seconds
            this.timer.Elapsed += (sender, args) => { OnBuildCacheUpdated(null, null); };
            BuildCache.CacheUpdated += OnBuildCacheUpdated;
        }

        #endregion

        #region Implementation of ICloseable

        public void OnClose()
        {
            BuildCache.CacheUpdated -= OnBuildCacheUpdated;
            this.timer?.Stop();
            this.timer?.Close();
            this.timer?.Dispose();
        }

        #endregion

        #region Public Properties

        public BuildCache BuildCache { get; }

        #endregion

        #region Private Methods

        private void OnBuildCacheUpdated(Object sender, EventArgs eventArgs)
        {
            this.timer?.Stop();
            foreach (var buildStatus in BuildCache.BuildsStatus)
            {
                buildStatus.CurrentBuild.BuildTime = DateTime.UtcNow;
            }
            this.timer?.Start();
        }

        #endregion

        #region Private Fields

        private Timer timer;

        #endregion
    }
}