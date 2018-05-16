using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Reflection;

using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

using Prism.Commands;

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class GeneralSettingsViewModel : ViewModelBase, ICloseable, ISaveable
    {
        #region Constructors

        [ImportingConstructor]
        internal GeneralSettingsViewModel(GlobalSettingsContainer globalSettingsContainer, UpdateChecker updateChecker, IBuildMonitorAdvanced buildMonitor)
        {
            this.globalSettingsContainer = globalSettingsContainer;
            this.GeneralSettings = this.globalSettingsContainer.GeneralSettings.Clone();
            this.updateChecker = updateChecker;
            this.buildMonitor = buildMonitor;
            SaveCommand = new DelegateCommand(OnSave);
        }

        #endregion

        #region Implementation of ICloseable

        public void OnClose()
        {
            this.GeneralSettings = null;
        }

        #endregion

        #region Implementation of ISaveable

        public DelegateCommand SaveCommand { get; }

        public void OnSave()
        {
            this.globalSettingsContainer.GeneralSettings = GeneralSettings.Clone();
            this.globalSettingsContainer.Save();

            this.buildMonitor.Start(this.globalSettingsContainer.BuildMonitorSettingsContainer, this.globalSettingsContainer.GeneralSettings);
            this.buildMonitor.BeginPollingBuilds();

            this.updateChecker.Start();
        }

        #endregion

        #region Public Properties

        public String CurrentAppVersion
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly.Location != null)
                {
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    var version = fileVersionInfo.ProductVersion;
                    return version;
                }
                return assembly.GetName().Version.ToString();
            }
        }

        public GeneralSettings GeneralSettings { get; private set; }

        #endregion

        #region Private Fields

        private readonly GlobalSettingsContainer globalSettingsContainer;

        private readonly UpdateChecker updateChecker;
        private readonly IBuildMonitorAdvanced buildMonitor;

        #endregion
    }
}