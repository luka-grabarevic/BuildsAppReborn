using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

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
            Load();
        }

        #endregion

        #region Public Properties

        public SettingsContainer<BuildMonitorSettings> BuildMonitorSettingsContainer { get; private set; }

        /// <summary>
        /// Gets the general settings.
        /// </summary>
        /// <value>
        /// The general settings.
        /// </value>
        public GeneralSettings GeneralSettings => this.generalSettingsContainer.Single();

        #endregion

        #region Public Methods

        public void DiscardChanges()
        {
            Load();
        }

        public void Save()
        {
            BuildMonitorSettingsContainer.Save(this.buildMonitorSettingsFilePath);
            this.generalSettingsContainer.Save(this.generalSettingsFilePath);
        }

        #endregion

        #region Private Methods

        private void Load()
        {
            var buidSettings = "buildMonitorSettings.json";
            var generalsettingsJson = "generalSettings.json";

#if DEBUG
            buidSettings = "buildMonitorSettings_debug.json";
            generalsettingsJson = "generalSettings_debug.json";
#endif

            this.buildMonitorSettingsFilePath = Path.Combine(Consts.ApplicationUserProfileFolder, buidSettings);
            this.generalSettingsFilePath = Path.Combine(Consts.ApplicationUserProfileFolder, generalsettingsJson);

            BuildMonitorSettingsContainer = SettingsContainer<BuildMonitorSettings>.Load(this.buildMonitorSettingsFilePath);
            this.generalSettingsContainer = SettingsContainer<GeneralSettings>.Load(this.generalSettingsFilePath);

            if (!this.generalSettingsContainer.Any())
            {
                this.generalSettingsContainer.Add(new GeneralSettings());
            }
        }

        #endregion

        #region Private Fields

        private String buildMonitorSettingsFilePath;

        private SettingsContainer<GeneralSettings> generalSettingsContainer;

        private String generalSettingsFilePath;

        #endregion
    }
}