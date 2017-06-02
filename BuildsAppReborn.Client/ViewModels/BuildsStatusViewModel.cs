using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;
using BuildsAppReborn.Infrastructure.Wpf;
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
            HistoryClickCommand = new DelegateCommand<BuildItem>(a => Task.Run(() => OnHistoryClickCommand(a)));
            OpenArtifactCommand = new DelegateCommand<IArtifact>(a => Task.Run(() => OnOpenArtifactCommand(a)));
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
            using (new WaitingIndicator())
            {
                StartProcess(item?.Build?.PortalUrl);
            }
        }

        private void OnOpenArtifactCommand(IArtifact artifact)
        {
            if (artifact != null)
            {
                using (new WaitingIndicator())
                {
                    // when it is a drop location folder, tries to be jump into the sub folder if possible
                    // ToDo: extract the artifact types in enums etc, as this is too TFS specific
                    if (artifact.Type == "FilePath" &&
                        !String.IsNullOrWhiteSpace(artifact.Data) &&
                        !String.IsNullOrWhiteSpace(artifact.Name))
                    {
                        var combinedPath = Path.Combine(artifact.Data, artifact.Name);
                        if (Directory.Exists(combinedPath))
                        {
                            if (StartProcess(combinedPath))
                            {
                                return;
                            }
                        }
                    }

                    // fallback option to start the provided download URL
                    StartProcess(artifact.DownloadUrl);
                }
            }
        }

        private Boolean StartProcess(String url)
        {
            using (new WaitingIndicator())
            {
                if (!String.IsNullOrWhiteSpace(url))
                {
                    try
                    {
                        Process.Start(url);
                        return true;
                    }
                    catch (Exception exception)
                    {
                        this.logger.Warn("Exception on StartProcess", exception);
                        return false;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Private Fields

        private readonly ILog logger = LogManager.GetLogger(typeof(BuildsStatusViewModel));

        private readonly Timer timer;

        #endregion
    }
}