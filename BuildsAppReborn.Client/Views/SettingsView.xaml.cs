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
        public SettingsView()
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
        private SettingsViewModel ViewModel
        {
            set { SetValue(DataContextProperty, value); }
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar)
                {
                    return;
                }

                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            this.MenuToggleButton.IsChecked = false;
        }
    }
}