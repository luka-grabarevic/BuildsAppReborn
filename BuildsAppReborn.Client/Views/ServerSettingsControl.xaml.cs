using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.ViewModels;

namespace BuildsAppReborn.Client.Views
{
    [Export(typeof(ISubSettingsControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ServerSettingsControl : ISubSettingsControl
    {
        #region Constructors

        public ServerSettingsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Implementation of ISubSettingsControl

        public String Title => "Server Settings";

        public UInt32 Order => 0;

        #endregion

        #region Private Properties

        [Import]
        private ServerSettingsViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }

        #endregion
    }
}