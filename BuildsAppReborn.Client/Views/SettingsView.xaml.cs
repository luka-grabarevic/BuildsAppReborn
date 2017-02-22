using System;
using System.ComponentModel.Composition;

using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.ViewModels;

namespace BuildsAppReborn.Client.Views
{
    [Export(typeof(ISettingsView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SettingsView : ISettingsView
    {
        #region Constructors

        public SettingsView()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides of Base

        protected override void OnClosed(EventArgs e)
        {
            var closableVm = DataContext as ICloseable;
            closableVm?.OnClose();

            base.OnClosed(e);
        }

        #endregion

        #region Private Properties

        [Import]
        private ServerSettingsViewModel ViewModel
        {
            set
            {
                SetValue(DataContextProperty, value);
            }
        }

        #endregion
    }
}