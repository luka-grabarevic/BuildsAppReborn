using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
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
        private SettingsViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }

        #endregion

        #region Private Methods

        private void UIElement_OnPreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            this.MenuToggleButton.IsChecked = false;
        }

        #endregion
    }
}