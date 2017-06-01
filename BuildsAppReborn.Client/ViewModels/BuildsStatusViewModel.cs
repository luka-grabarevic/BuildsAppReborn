using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;
using log4net;
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
            OpenArtifactCommand = new DelegateCommand<IArtifact>(OnOpenArtifactCommand);
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

        public DelegateCommand<IArtifact> OpenArtifactCommand { get; set; }

        #endregion

        #region Private Methods

        private void OnBuildCacheUpdated(Object sender, EventArgs eventArgs)
        {
            this.timer?.Stop();
            foreach (var buildStatus in BuildCache.BuildsStatus.ToList())
                buildStatus.CurrentBuild.Refresh();
            this.timer?.Start();
        }

        private void OnHistoryClickCommand(BuildItem item)
        {
            StartProcess(item?.Build?.PortalUrl);
        }

        private void OnOpenArtifactCommand(IArtifact artifact)
        {
            StartProcess(artifact?.DownloadUrl);
        }

        private void StartProcess(String url)
        {
            if (!String.IsNullOrWhiteSpace(url))
                try
                {
                    Process.Start(url);
                }
                catch (Exception exception)
                {
                    this.logger.Warn("Exception on StartProcess", exception);
                }
        }

        #endregion

        #region Private Fields

        private readonly ILog logger = LogManager.GetLogger(typeof(BuildsStatusViewModel));

        private readonly Timer timer;

        #endregion
    }
}