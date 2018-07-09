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
        public GlobalSettingsContainer()
        {
            Load();
        }

        public SettingsContainer<BuildMonitorSettings> BuildMonitorSettingsContainer { get; set; }

        /// <summary>
        /// Gets the general settings.
        /// </summary>
        /// <value>
        /// The general settings.
        /// </value>
        [Export]
        public GeneralSettings GeneralSettings
        {
            get { return this.generalSettingsContainer.Single(); }
            set
            {
                this.generalSettingsContainer.Clear();
                this.generalSettingsContainer.Add(value);
            }
        }

        public void Save()
        {
            BuildMonitorSettingsContainer.Save(this.buildMonitorSettingsFilePath);
            this.generalSettingsContainer.Save(this.generalSettingsFilePath);
        }

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

        private String buildMonitorSettingsFilePath;

        private SettingsContainer<GeneralSettings> generalSettingsContainer;

        private String generalSettingsFilePath;
    }
}