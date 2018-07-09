using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using BuildsAppReborn.Client.Properties;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;
using BuildsAppReborn.Infrastructure;
using log4net;
using Squirrel;

namespace BuildsAppReborn.Client
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class UpdateChecker
    {
        [ImportingConstructor]
        public UpdateChecker(GlobalSettingsContainer globalSettingsContainer, LazyContainer<INotificationProvider, IPriorityMetadata> notificationProviders)
        {
            this.globalSettingsContainer = globalSettingsContainer;

            this.timer.Elapsed += TimerOnElapsed;

            this.notificationProvider = notificationProviders.GetSupportedNotificationProvider();
        }

        public void Start()
        {
            Stop();

            if (!GeneralSettings.CheckForUpdates)
            {
                return;
            }

            this.timer.Interval = GeneralSettings.UpdateCheckInterval.TotalMilliseconds;
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        public void UpdateCheck(Boolean manualCheck)
        {
            Task.Run(() => UpdateCheckInternalAsync(manualCheck));
        }

        private GeneralSettings GeneralSettings => this.globalSettingsContainer.GeneralSettings;

        private void TimerOnElapsed(Object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Task.Run(() => UpdateCheckInternalAsync(false));
        }

        private async Task UpdateCheckInternalAsync(Boolean manualCheck)
        {
#if DEBUG
            return;
#endif

            if (!GeneralSettings.CheckForUpdates)
            {
                return;
            }

            this.logger.Info("Checking for update...");
            try
            {
                var repo = Settings.Default.UpdateCheckUrl;
                using (var updateMgrTask = UpdateManager.GitHubUpdateManager(repo, null, null, null, GeneralSettings.IncludePreReleases))
                {
                    using (var updateManager = await updateMgrTask.ConfigureAwait(false))
                    {
                        var updateInfo = await updateManager.CheckForUpdate().ConfigureAwait(false);
                        if (updateInfo.ReleasesToApply.Any())
                        {
                            this.logger.Info("New Update found!");
                            if (!GeneralSettings.AutoInstall)
                            {
                                if (GeneralSettings.NotifyOnNewUpdate)
                                {
                                    // ToDo: show if new update available
                                }

                                // ToDo: implement update when user accepts
                            }
                            else
                            {
                                this.logger.Debug("Auto installing update...");
                                await updateManager.DownloadReleases(updateInfo.ReleasesToApply).ConfigureAwait(false);
                                var path = await updateManager.ApplyReleases(updateInfo).ConfigureAwait(false);
                                if (path != null)
                                {
                                    this.logger.Debug("Update install finished.");
                                    if (GeneralSettings.NotifyOnNewUpdate)
                                    {
                                        this.notificationProvider?.ShowMessage($"{this.version.ProductName} - Update check finished!",
                                                                               "New Updates installed. Click here to restart.",
                                                                               () => UpdateManager.RestartApp(Path.Combine(path, Path.GetFileName(entryAssembly.Location))));
                                    }
                                }
                                else
                                {
                                    this.logger.Debug("Update install failed.");
                                }
                            }
                        }
                        else
                        {
                            this.logger.Info("No Update found!");
                            if (manualCheck)
                            {
                                this.notificationProvider?.ShowMessage($"{this.version.ProductName} - Update check finished!", "No updates found!");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error("Error while checking for update", exception);
                if (GeneralSettings.NotifyOnNewUpdate)
                {
                    this.notificationProvider?.ShowMessage($"{this.version.ProductName} Update Check failed!", "See log file for more information.");
                }
            }
        }

        private static readonly Assembly entryAssembly = Assembly.GetEntryAssembly();

        private readonly GlobalSettingsContainer globalSettingsContainer;
        private readonly ILog logger = LogManager.GetLogger(typeof(UpdateChecker));
        private readonly INotificationProvider notificationProvider;
        private readonly Timer timer = new Timer();
        private readonly FileVersionInfo version = FileVersionInfo.GetVersionInfo(entryAssembly.Location);
    }
}