using System;

namespace BuildsAppReborn.Contracts.Models
{
    public class GeneralSettings
    {
        #region Constructors

        public GeneralSettings()
        {
            // currently default values
            AutoInstall = true;
            CheckForUpdates = true;
            NotifyOnNewUpdate = true;
            IncludePreReleases = true;

            UpdateCheckInterval = TimeSpan.FromHours(1);
            PollingInterval = TimeSpan.FromMinutes(1);
        }

        #endregion

        #region Public Properties

        public Boolean AutoInstall { get; set; }

        public Boolean CheckForUpdates { get; set; }

        public Boolean IncludePreReleases { get; set; }

        public Boolean NotifyOnNewUpdate { get; set; }

        public TimeSpan UpdateCheckInterval { get; set; }

        public BuildViewStyle ViewStyle { get; set; }

        public TimeSpan PollingInterval { get; set; }

        #endregion
    }
}