using System;
using System.ComponentModel.Composition;
using System.Reflection;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class GeneralSettingsViewModel : ViewModelBase, ICloseable
    {
        #region Constructors

        [ImportingConstructor]
        internal GeneralSettingsViewModel(GlobalSettingsContainer globalSettingsContainer, UpdateChecker updateChecker)
        {
            this.globalSettingsContainer = globalSettingsContainer;
            this.updateChecker = updateChecker;
        }

        #endregion

        #region Implementation of ICloseable

        public void OnClose()
        {
            this.updateChecker.Start();
        }

        #endregion

        #region Public Properties

        public Version CurrentAppVersion => Assembly.GetEntryAssembly().GetName().Version;

        public GeneralSettings GeneralSettings => this.globalSettingsContainer.GeneralSettings;

        #endregion

        #region Private Fields

        private readonly GlobalSettingsContainer globalSettingsContainer;
        private readonly UpdateChecker updateChecker;

        #endregion
    }
}