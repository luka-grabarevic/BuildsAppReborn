using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public GeneralSettings GeneralSettings => this.generalSettingsContainer.Single();

        public void Save()
        {
            BuildMonitorSettingsContainer.Save(this.buildMonitorSettingsFilePath);
            this.generalSettingsContainer.Save(this.generalSettingsFilePath);
        }

        public void Update(GeneralSettings generalSettings)
        {
            foreach (var property in this.generalSettingsProperties)
            {
                property.SetValue(GeneralSettings, property.GetValue(generalSettings));
            }
        }

        private void Load()
        {
            var buildSettings = "buildMonitorSettings.json";
            var generalSettingsJson = "generalSettings.json";

#if DEBUG
            buildSettings = "buildMonitorSettings_debug.json";
            generalSettingsJson = "generalSettings_debug.json";
#endif

            this.buildMonitorSettingsFilePath = Path.Combine(Consts.ApplicationUserProfileFolder, buildSettings);
            this.generalSettingsFilePath = Path.Combine(Consts.ApplicationUserProfileFolder, generalSettingsJson);

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

        private readonly PropertyInfo[] generalSettingsProperties =
            typeof(GeneralSettings).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(a => a.CanWrite).ToArray();
    }
}