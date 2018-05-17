using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.ViewModels;

namespace BuildsAppReborn.Client.Views
{
    [Export(typeof(IBuildsStatusView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class BuildsStatusView : IBuildsStatusView
    {
        public BuildsStatusView()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            var closableVm = DataContext as ICloseable;
            closableVm?.OnClose();

            base.OnClosed(e);
        }

        [Import]
        private BuildsStatusViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }
    }
}