using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;

using Prism.Commands;

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ServerSettingsViewModel : ViewModelBase, ISaveable, ICloseable
    {
        #region Constructors

        [ImportingConstructor]
        internal ServerSettingsViewModel(GlobalSettingsContainer globalSettingsContainer, ExportFactoryContainer<IBuildProviderView, IIdentifierMetadata> buildProviderViews, IBuildMonitorAdvanced buildMonitor)
        {
            this.globalSettingsContainer = globalSettingsContainer;
            this.buildMonitor = buildMonitor;
            BuildProviderViews = buildProviderViews;

            Views = new ObservableCollection<IBuildProviderView>();

            InitializeSettings();

            SaveCommand = new DelegateCommand(OnSave);
            AddProviderCommand = new DelegateCommand<IIdentifierMetadata>(OnAddProvider);
            RemoveProviderCommand = new DelegateCommand<IBuildProviderView>(OnRemoveView);
        }

        #endregion

        #region Implementation of ICloseable

        public void OnClose()
        {
            this.buildMonitorSettingsContainer = null;
        }

        #endregion

        #region Implementation of ISaveable

        public DelegateCommand SaveCommand { get; }

        public void OnSave()
        {
            this.globalSettingsContainer.BuildMonitorSettingsContainer = this.buildMonitorSettingsContainer.Clone();
            this.globalSettingsContainer.Save();
            this.buildMonitor.Start(this.globalSettingsContainer.BuildMonitorSettingsContainer, this.globalSettingsContainer.GeneralSettings, TimeSpan.FromMinutes(1));
            this.buildMonitor.BeginPollingBuilds();
        }

        #endregion

        #region Public Properties

        public DelegateCommand<IIdentifierMetadata> AddProviderCommand { get; }

        public IEnumerable<IIdentifierMetadata> AvailableProvider => BuildProviderViews.MetaData;

        public ExportFactoryContainer<IBuildProviderView, IIdentifierMetadata> BuildProviderViews { get; }

        public DelegateCommand<IBuildProviderView> RemoveProviderCommand { get; }

        public ObservableCollection<IBuildProviderView> Views { get; }

        #endregion

        #region Private Methods

        private void AddView(String providerid, BuildMonitorSettings buildMonitorSettings)
        {
            var providerView = BuildProviderViews.GetSingleOrDefault(a => a.Id == providerid);
            if (providerView != null)
            {
                var view = providerView.CreateExport().Value;

                view.ViewModel.Initialize(buildMonitorSettings);
                Views.Add(view);
            }
        }

        private void InitializeSettings()
        {
            this.buildMonitorSettingsContainer = this.globalSettingsContainer.BuildMonitorSettingsContainer.Clone();
            foreach (var buildMonitorSettings in this.buildMonitorSettingsContainer)
            {
                var providerid = buildMonitorSettings.BuildProviderId;
                AddView(providerid, buildMonitorSettings);
            }
        }

        private void OnAddProvider(IIdentifierMetadata selectedProvider)
        {
            var providerid = selectedProvider.Id;
            var buildMonitorSettings = new BuildMonitorSettings(providerid);
            this.buildMonitorSettingsContainer.Add(buildMonitorSettings);

            AddView(providerid, buildMonitorSettings);
        }

        private void OnRemoveView(IBuildProviderView view)
        {
            if (view != null)
            {
                Views.Remove(view);

                var settings = this.buildMonitorSettingsContainer.SingleOrDefault(a => a.UniqueId == view.ViewModel.MonitorSettings.UniqueId);
                if (settings != null)
                {
                    this.buildMonitorSettingsContainer.Remove(settings);
                }
            }
        }

        #endregion

        #region Private Fields

        private readonly IBuildMonitorAdvanced buildMonitor;

        private SettingsContainer<BuildMonitorSettings> buildMonitorSettingsContainer;

        private readonly GlobalSettingsContainer globalSettingsContainer;

        #endregion
    }
}