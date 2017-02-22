using System;
using System.ComponentModel.Composition;
using System.IO;

using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client
{
    [Export(typeof(GlobalSettingsContainer))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class GlobalSettingsContainer
    {
        #region Constructors

        public GlobalSettingsContainer()
        {
            this.settingsFilePath = Path.Combine(Consts.ApplicationUserProfileFolder, "settings.json");
            BuildMonitorSettingsContainer = SettingsContainer<BuildMonitorSettings>.Load(this.settingsFilePath);
        }

        #endregion

        #region Public Properties

        public SettingsContainer<BuildMonitorSettings> BuildMonitorSettingsContainer { get; }

        #endregion

        #region Public Methods

        public void Save()
        {
            BuildMonitorSettingsContainer.Save(this.settingsFilePath);
        }

        #endregion

        #region Private Fields

        private String settingsFilePath;

        #endregion
    }
}