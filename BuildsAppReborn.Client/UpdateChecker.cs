using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
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
        #region Constructors

        [ImportingConstructor]
        public UpdateChecker(GlobalSettingsContainer globalSettingsContainer, LazyContainer<INotificationProvider, IPriorityMetadata> notificationProviders)
        {
            this.globalSettingsContainer = globalSettingsContainer;

            this.timer.Elapsed += TimerOnElapsed;

            this.notificationProvider = notificationProviders.GetSupportedNotificationProvider();
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            Stop();

            if (!GeneralSettings.CheckForUpdates) return;

            this.timer.Interval = GeneralSettings.UpdateCheckInterval.TotalMilliseconds;
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        public async void UpdateCheck()
        {
            await UpdateCheckInternal();
        }

        #endregion

        #region Private Properties

        private GeneralSettings GeneralSettings => this.globalSettingsContainer.GeneralSettings;

        #endregion

        #region Private Methods

        private async void TimerOnElapsed(Object sender, ElapsedEventArgs elapsedEventArgs)
        {
            await UpdateCheckInternal();
        }

        private async Task UpdateCheckInternal()
        {
#if DEBUG
            return;
#endif

            if (!GeneralSettings.CheckForUpdates) return;

            this.logger.Info("Checking for update...");
            try
            {
                var repo = Settings.Default.UpdateCheckUrl;
                using (var updateMgrTask = UpdateManager.GitHubUpdateManager(repo, null, null, null, GeneralSettings.IncludePreReleases))
                {
                    using (var updateManager = await updateMgrTask)
                    {
                        if (!GeneralSettings.AutoInstall)
                        {
                            var updateInfo = await updateManager.CheckForUpdate();
                            if (GeneralSettings.NotifyOnNewUpdate)
                            {
                                // ToDo: show if new update available
                            }
                            // ToDo: implement update when user accepts
                        }
                        else
                        {
                            var result = await updateManager.UpdateApp();
                            if (result != null)
                            {
                                this.logger.Info("New Update found!");
                                if (GeneralSettings.NotifyOnNewUpdate)
                                {
                                    this.notificationProvider?.ShowMessage($"{this.version.ProductName} New update found!", "Update will be installed automatically on next start.");
                                }
                            }
                            else
                            {
                                this.logger.Info("No Update found!");
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

        #endregion

        #region Private Fields

        private readonly GlobalSettingsContainer globalSettingsContainer;
        private ILog logger = LogManager.GetLogger(typeof(UpdateChecker));
        private INotificationProvider notificationProvider;
        private Timer timer = new Timer();
        private FileVersionInfo version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        #endregion
    }
}