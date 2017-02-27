using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.ViewModels;

namespace BuildsAppReborn.Client.Views
{
    [Export(typeof(ISubSettingsControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class GeneralSettingsControl : ISubSettingsControl
    {
        #region Constructors

        public GeneralSettingsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Implementation of ISubSettingsControl

        public UInt32 Order => 0;

        public String Title => "General Settings";

        #endregion

        [Import]
        private GeneralSettingsViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }
    }
}