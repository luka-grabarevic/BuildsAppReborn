using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.Notification;
using BuildsAppReborn.Client.Resources;
using BuildsAppReborn.Infrastructure;
using Prism.Commands;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    public class NotifyIconViewModel : ViewModelBase
    {
        #region Constructors

        [ImportingConstructor]
        public NotifyIconViewModel(ExportFactory<IBuildsStatusView> buildsExportFactory, ExportFactory<ISettingsView> settingsExportFactory, BuildCache buildCache, NotificationManager notificationManager)
        {
            if (buildCache.CacheStatus == BuildCacheStatus.NotConfigured) TrayIcon = IconProvider.SettingsIcon;
            else TrayIcon = IconProvider.LoadingIcon;

            this.buildsExportFactory = buildsExportFactory;
            this.settingsExportFactory = settingsExportFactory;
            buildCache.CacheUpdated += (sender, args) => { TrayIcon = buildCache.CurrentIcon; };

            Initialize();
        }

        #endregion

        #region Public Properties

        public ICommand ExitApplicationCommand { get; private set; }

        public ICommand ShowSettingsWindowCommand { get; private set; }

        public ICommand ShowWindowCommand { get; private set; }

        public String TrayIcon
        {
            get { return this.trayIcon; }
            set
            {
                if (this.trayIcon != value)
                {
                    this.trayIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Internal Static Methods

        internal static void OpenWindow<T>(ExportFactory<T> newWindow)
        {
            var currentMainWindow = Application.Current.MainWindow;
            if (currentMainWindow is T)
            {
                if (currentMainWindow.WindowState == WindowState.Minimized)
                {
                    currentMainWindow.WindowState = WindowState.Normal;
                }
                if (!currentMainWindow.IsActive)
                {
                    currentMainWindow.Show();
                }
                currentMainWindow.Activate();
                return;
            }
            Window buildStatusWindow = null;
            foreach (var window in Application.Current.Windows)
            {
                if (window as Window is T)
                {
                    buildStatusWindow = window as Window;
                    break;
                }
            }
            if (buildStatusWindow == null)
            {
                (newWindow.CreateExport().Value as Window)?.Show();
            }
            if (buildStatusWindow != null)
            {
                if (buildStatusWindow.WindowState == WindowState.Minimized)
                {
                    buildStatusWindow.WindowState = WindowState.Normal;
                }
                if (currentMainWindow == null || !currentMainWindow.IsActive)
                {
                    buildStatusWindow.Show();
                }
                buildStatusWindow.Activate();
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            ShowWindowCommand = new DelegateCommand(() => { OpenWindow<IBuildsStatusView>(this.buildsExportFactory); });

            ShowSettingsWindowCommand = new DelegateCommand(() => { OpenWindow<ISettingsView>(this.settingsExportFactory); });

            ExitApplicationCommand = new DelegateCommand(() => Application.Current.Shutdown());
        }

        #endregion

        #region Private Fields

        private readonly ExportFactory<IBuildsStatusView> buildsExportFactory;

        private readonly ExportFactory<ISettingsView> settingsExportFactory;
        private String trayIcon;

        #endregion
    }
}