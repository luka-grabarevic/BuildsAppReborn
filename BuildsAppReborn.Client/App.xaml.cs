using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using BuildsAppReborn.Client.ViewModels;
using BuildsAppReborn.Contracts;
using Hardcodet.Wpf.TaskbarNotification;

namespace BuildsAppReborn.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Overrides of Base

        protected override void OnExit(ExitEventArgs e)
        {
            this.buildMonitor.Stop();
            this.globalSettingsContainer.Save();
            this.notifyIcon.Dispose();
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EnsureOnlyOneInstance();

            var compositionContainer = BuildCompositionContainer();
            this.globalSettingsContainer = compositionContainer.GetExportedValue<GlobalSettingsContainer>();

            this.buildMonitor = compositionContainer.GetExportedValue<IBuildMonitorAdvanced>();
            this.buildMonitor.Start(this.globalSettingsContainer.BuildMonitorSettingsContainer, TimeSpan.FromMinutes(1));

            this.notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");
            if (this.notifyIcon != null)
            {
                this.notifyIcon.DataContext = compositionContainer.GetExportedValue<NotifyIconViewModel>();
            }
        }

        #endregion

        #region Private Static Methods

        private static CompositionContainer BuildCompositionContainer()
        {
            var catalog = new AggregateCatalog();

            var directoryName = Path.GetDirectoryName(typeof(App).Assembly.Location);
            if (directoryName != null)
            {
                var assemblies = Directory.GetFiles(directoryName, "*.dll").Union(Directory.GetFiles(directoryName, "*.exe"))
                    .Where(a => !String.Equals(Path.GetFileName(a), "NuGet.Squirrel.dll", StringComparison.InvariantCultureIgnoreCase));

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return new CompositionContainer(catalog, true);
        }

        private static void EnsureOnlyOneInstance()
        {
            var currentProcess = Process.GetCurrentProcess();
            var count = Process.GetProcesses().Count(p => p.ProcessName == currentProcess.ProcessName);
            if (count > 1)
            {
                // MessageBox.Show("An instance is already running ...");
                Current.Shutdown();
            }
        }

        #endregion

        #region Private Fields

        private IBuildMonitorAdvanced buildMonitor;

        private GlobalSettingsContainer globalSettingsContainer;

        private TaskbarIcon notifyIcon;

        #endregion
    }
}