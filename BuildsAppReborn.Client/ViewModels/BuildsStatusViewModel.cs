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
        [ImportingConstructor]
        public BuildsStatusViewModel(BuildCache buildCache)
        {
            BuildCache = buildCache;
            this.timer = new Timer {Interval = 10000, AutoReset = true}; // update every 10 seconds
            this.timer.Elapsed += (sender, args) => { OnBuildCacheUpdated(null, null); };
            BuildCache.CacheUpdated += OnBuildCacheUpdated;

            ProgressMinimum = 0;
            ProgressMaximum = BuildCache.ProgressComponent.MaximumProgress;
            BuildCache.ProgressComponent.ProgressUpdated += progress => Progress = progress;

            HistoryClickCommand = new DelegateCommand<BuildItem>(a => Task.Run(() => StartProcess(a?.Build?.WebUrl)));
            OpenArtifactCommand = new DelegateCommand<IArtifact>(a => Task.Run(() => OnOpenArtifactCommand(a)));
            TestRunClickCommand = new DelegateCommand<ITestRun>(a => Task.Run(() => StartProcess(a?.WebUrl)));
        }

        public BuildCache BuildCache { get; }

        public DelegateCommand<BuildItem> HistoryClickCommand { get; set; }

        public DelegateCommand<IArtifact> OpenArtifactCommand { get; set; }

        public Double Progress
        {
            get { return this.progress; }
            set
            {
                this.progress = value;
                OnPropertyChanged();
            }
        }

        public Double ProgressMaximum
        {
            get { return this.progressMaximum; }
            set
            {
                this.progressMaximum = value;
                OnPropertyChanged();
            }
        }

        public Double ProgressMinimum
        {
            get { return this.progressMinimum; }
            set
            {
                this.progressMinimum = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand<ITestRun> TestRunClickCommand { get; set; }

        public void OnClose()
        {
            BuildCache.CacheUpdated -= OnBuildCacheUpdated;
            this.timer?.Stop();
            this.timer?.Close();
            this.timer?.Dispose();
        }

        private void OnBuildCacheUpdated(Object sender, EventArgs eventArgs)
        {
            this.timer?.Stop();
            foreach (var buildStatus in BuildCache.BuildsStatus.ToList())
            {
                buildStatus.CurrentBuild.Refresh();
            }

            this.timer?.Start();
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

        private readonly ILog logger = LogManager.GetLogger(typeof(BuildsStatusViewModel));
        private Double progress;
        private Double progressMaximum;
        private Double progressMinimum;

        private readonly Timer timer;
    }
}