using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Client.Notification;
using BuildsAppReborn.Client.Resources;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Prism.Commands;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NotifyIconViewModel : ViewModelBase
    {
        #region Constructors

        [ImportingConstructor]
        internal NotifyIconViewModel(ExportFactory<IBuildsStatusView> buildsExportFactory, ExportFactory<ISettingsView> settingsExportFactory, BuildCache buildCache, NotificationManager notificationManager, UpdateChecker updateChecker, GlobalSettingsContainer settingsContainer)
        {
            if (buildCache.CacheStatus == BuildCacheStatus.NotConfigured)
            {
                TrayIcon = IconProvider.SettingsIcon;
            }
            else
            {
                TrayIcon = IconProvider.LoadingIcon;
            }

            this.buildsExportFactory = buildsExportFactory;
            this.settingsExportFactory = settingsExportFactory;
            this.updateChecker = updateChecker;
            globalSettingsContainer = settingsContainer;
            buildCache.CacheUpdated += (sender, args) => { TrayIcon = buildCache.CurrentIcon; };

            Initialize();
        }

        #endregion

        #region Public Properties

        public ICommand CheckUpdateCommand { get; private set; }

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

        #region Internal Methods

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
                var window = newWindow.CreateExport().Value as Window;
                if (window != null)
                {
                    RestoreWindowSettings(window, globalSettingsContainer.GeneralSettings.WindowSettings);
                    window.Closed += WindowOnClosed;
                    window.Show();
                }
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

            CheckUpdateCommand = new DelegateCommand(() => this.updateChecker.UpdateCheck(true));
        }

        private static void RestoreWindowSettings(Window window, List<WindowSetting> windowSettings)
        {
            if (window == null || windowSettings == null)
            {
                return;
            }

            var windowId = window.GetType().FullName;
            var setting = windowSettings.FirstOrDefault(f => f.Id == windowId);
            if (setting == null)
            {
                return;
            }

            window.Top = setting.Top;
            window.Left = setting.Left;
            window.Height = setting.Height;
            window.Width = setting.Width;
            window.WindowState = setting.WindowState;
        }

        private static void SaveWindowSettings(Window window, List<WindowSetting> windowSettings)
        {
            if (window == null || windowSettings == null)
            {
                return;
            }

            var windowId = window.GetType().FullName;
            var setting = windowSettings.FirstOrDefault(f => f.Id == windowId);
            if (setting == null)
            {
                setting = new WindowSetting {Id = windowId};
                globalSettingsContainer.GeneralSettings.WindowSettings.Add(setting);
            }

            setting.Top = window.Top;
            setting.Left = window.Left;
            setting.Height = window.Height;
            setting.Width = window.Width;
            setting.WindowState = window.WindowState;
            globalSettingsContainer.Save();
        }

        private static void WindowOnClosed(Object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window == null)
            {
                return;
            }

            SaveWindowSettings(window, globalSettingsContainer.GeneralSettings.WindowSettings);

            window.Closed -= WindowOnClosed;
        }

        #endregion

        #region Private Fields

        private readonly ExportFactory<IBuildsStatusView> buildsExportFactory;
        private static GlobalSettingsContainer globalSettingsContainer;
        private readonly ExportFactory<ISettingsView> settingsExportFactory;
        private String trayIcon;
        private readonly UpdateChecker updateChecker;

        #endregion
    }
}