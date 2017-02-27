using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using BuildsAppReborn.Client.Properties;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;
using Squirrel;

namespace BuildsAppReborn.Client
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class UpdateChecker
    {
        #region Constructors

        [ImportingConstructor]
        public UpdateChecker(GlobalSettingsContainer globalSettingsContainer, [ImportMany] IEnumerable<INotificationProvider> notificationProviders)
        {
            this.globalSettingsContainer = globalSettingsContainer;

            this.timer.Elapsed += TimerOnElapsed;

            // ToDo: find the compatible provider for this system
            this.notificationProvider = notificationProviders.FirstOrDefault();
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
            if (!GeneralSettings.CheckForUpdates) return;
            if (GeneralSettings.NotifyOnNewUpdate) this.notificationProvider.ShowMessage($"{this.version.ProductName} Update Check started", "Checking for new updates...");
            try
            {
                var repo = Settings.Default.UpdateCheckUrl;
                var updateMgrTask = UpdateManager.GitHubUpdateManager(repo, null, null, null, GeneralSettings.IncludePreReleases);

                var updateManager = await updateMgrTask;

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
                    if (GeneralSettings.NotifyOnNewUpdate)
                    {
                        if (result == null) this.notificationProvider.ShowMessage($"{this.version.ProductName} Update Check finished", "Currently no new updates");
                        else this.notificationProvider.ShowMessage($"{this.version.ProductName} New update found!", "Update will be installed automatically on next start.");
                    }
                }
            }
            catch (Exception exception)
            {
                // ToDo: add proper logging and change message text
                if (GeneralSettings.NotifyOnNewUpdate) this.notificationProvider.ShowMessage($"{this.version.ProductName} Update Check failed!", "See (currently not existing) log file.");
            }
        }

        #endregion

        #region Private Fields

        private readonly GlobalSettingsContainer globalSettingsContainer;
        private INotificationProvider notificationProvider;
        private Timer timer = new Timer();
        private FileVersionInfo version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        #endregion
    }
}