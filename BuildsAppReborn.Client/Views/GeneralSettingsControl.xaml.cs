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
        public GeneralSettingsControl()
        {
            InitializeComponent();
        }

        public UInt32 Order => 0;

        public String Title => "General Settings";

        [Import]
        private GeneralSettingsViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }
    }
}