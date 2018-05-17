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
        public ServerSettingsControl()
        {
            InitializeComponent();
        }

        public UInt32 Order => 1;

        public String Title => "Server Settings";

        [Import]
        private ServerSettingsViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }
    }
}