using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;
using Prism.Commands;

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
            HistoryClickCommand = new DelegateCommand<BuildItem>(OnHistoryClickCommand);
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

        public DelegateCommand<BuildItem> HistoryClickCommand { get; set; }

        #endregion

        #region Private Methods

        private void OnBuildCacheUpdated(Object sender, EventArgs eventArgs)
        {
            this.timer?.Stop();
            foreach (var buildStatus in BuildCache.BuildsStatus.ToList())
            {
                buildStatus.CurrentBuild.BuildTime = DateTime.UtcNow;
            }
            this.timer?.Start();
        }

        private void OnHistoryClickCommand(BuildItem item)
        {
            var url = item?.Build?.PortalUrl;
            if (!String.IsNullOrWhiteSpace(url))
            {
                Process.Start(url);
            }
        }

        #endregion

        #region Private Fields

        private Timer timer;

        #endregion
    }
}