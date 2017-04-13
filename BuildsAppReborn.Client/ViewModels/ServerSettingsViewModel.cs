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

using Dragablz;

using Prism.Commands;

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ServerSettingsViewModel : ViewModelBase, ICloseable
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

            AddProviderCommand = new DelegateCommand<IIdentifierMetadata>(OnAddProvider);

            ClosingItemCallback += args => OnRemoveView(args.DragablzItem.DataContext as IBuildProviderView);
        }

        #endregion

        #region Implementation of ICloseable

        public void OnClose()
        {
            this.globalSettingsContainer.Save();
            this.buildMonitor.Start(this.globalSettingsContainer.BuildMonitorSettingsContainer, TimeSpan.FromMinutes(1));
            this.buildMonitor.BeginPollingBuilds();
        }

        #endregion

        #region Public Properties

        public DelegateCommand<IIdentifierMetadata> AddProviderCommand { get; }

        public IEnumerable<IIdentifierMetadata> AvailableProvider => BuildProviderViews.MetaData;

        public ExportFactoryContainer<IBuildProviderView, IIdentifierMetadata> BuildProviderViews { get; }

        public ItemActionCallback ClosingItemCallback { get; }

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
            foreach (var buildMonitorSettings in this.globalSettingsContainer.BuildMonitorSettingsContainer)
            {
                var providerid = buildMonitorSettings.BuildProviderId;
                AddView(providerid, buildMonitorSettings);
            }
        }

        private void OnAddProvider(IIdentifierMetadata selectedProvider)
        {
            var providerid = selectedProvider.Id;
            var buildMonitorSettings = new BuildMonitorSettings(providerid);
            this.globalSettingsContainer.BuildMonitorSettingsContainer.Add(buildMonitorSettings);

            AddView(providerid, buildMonitorSettings);
        }

        private void OnRemoveView(IBuildProviderView view)
        {
            if (view != null)
            {
                Views.Remove(view);

                var settings = this.globalSettingsContainer.BuildMonitorSettingsContainer.SingleOrDefault(a => a.UniqueId == view.ViewModel.MonitorSettings.UniqueId);
                if (settings != null)
                {
                    this.globalSettingsContainer.BuildMonitorSettingsContainer.Remove(settings);
                }
            }
        }

        #endregion

        #region Private Fields

        private readonly IBuildMonitorAdvanced buildMonitor;

        private readonly GlobalSettingsContainer globalSettingsContainer;

        #endregion
    }
}