using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using BuildsAppReborn.Access.UI.ViewModel.SubViewModels;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using BuildsAppReborn.Infrastructure.Collections;
using Prism.Commands;

namespace BuildsAppReborn.Access.UI.ViewModel
{
    internal abstract class TfsBuildProviderViewModelBase : ProviderViewModelBase
    {
        protected TfsBuildProviderViewModelBase(IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer)
        {
            this.buildDefinitionEqualityComparer = buildDefinitionEqualityComparer;
            BuildDefinitions = new RangeObservableCollection<BuildDefinitionViewModel>();
            BuildDefinitions.CollectionChanged += (sender, args) => SetDisplayName(BuildDefinitions.Select(a => a.BuildDefinition));
            InitializeCommands();
        }

        public String AccessToken
        {
            // never show access token once provided
            get { return String.Empty; }
            set
            {
                // ToDo: Token should only be saved if connection was successful
                if (!String.IsNullOrWhiteSpace(value))
                {
                    MonitorSettings[TfsBuildProviderBase.PersonalAccessTokenSettingsKey] = value;
                    OnPropertyChanged();
                    ConnectCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public RangeObservableCollection<BuildDefinitionViewModel> BuildDefinitions { get; }

        public DelegateCommand ConnectCommand { get; private set; }

        public override String DisplayName
        {
            get { return String.IsNullOrWhiteSpace(this.displayName) ? ProviderName : this.displayName; }
            protected set
            {
                this.displayName = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsConnecting
        {
            get { return this.isConnecting; }
            set
            {
                this.isConnecting = value;
                OnPropertyChanged();
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        public String ProjectUrl
        {
            get { return MonitorSettings?.GetDefaultValueIfNotExists<String>(TfsBuildProviderBase.ProjectUrlSettingKey); }
            set
            {
                MonitorSettings[TfsBuildProviderBase.ProjectUrlSettingKey] = value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Url));
                ConnectCommand?.RaiseCanExecuteChanged();
            }
        }

        public override ObservableCollection<IBuildDefinition> SelectedBuildDefinitions => this.selectedBuildDefinitions;

        public Boolean ShowPersonalAccessTokenInput
        {
            get { return this.showPersonalAccessTokenInput; }
            set
            {
                OnPropertyChanged();
                this.showPersonalAccessTokenInput = value;
            }
        }

        public String StatusText
        {
            get { return this.statusText; }
            set
            {
                this.statusText = value;
                OnPropertyChanged();
            }
        }

        public override String Url => ProjectUrl;

        public override void Save()
        {
            if (IsInEditMode)
            {
                base.Save();

                MonitorSettings.SelectedBuildDefinitions.Clear();
                var selected = BuildDefinitions.Where(a => a.IsSelected && !a.IsDeleted);
                foreach (var viewModel in selected)
                {
                    MonitorSettings.SelectedBuildDefinitions.Add(viewModel.BuildDefinition);
                }

                IsInEditMode = false;

                UpdateSelectedCollection();
                BuildDefinitions.Clear();
            }
        }

        protected abstract String ProviderName { get; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!MonitorSettings.ContainsKey(TfsBuildProviderBase.ProjectCredentialsSettingKey))
            {
                MonitorSettings[TfsBuildProviderBase.ProjectCredentialsSettingKey] = CredentialCache.DefaultCredentials;
            }

            SetDisplayName(MonitorSettings.SelectedBuildDefinitions);

            UpdateSelectedCollection();

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(ProjectUrl));
            ConnectCommand?.RaiseCanExecuteChanged();

            if (!SupportsDefaultCredentials && SupportsPersonalAccessToken)
            {
                ShowPersonalAccessTokenInput = true;
                if (MonitorSettings.ContainsKey(TfsBuildProviderBase.PersonalAccessTokenSettingsKey))
                {
                    var token = MonitorSettings?.GetDefaultValueIfNotExists<String>(TfsBuildProviderBase.PersonalAccessTokenSettingsKey);
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                        ShowPersonalAccessTokenInput = false;
                    }
                }
            }
        }

        private void ApplyBuildDefinitions(IEnumerable<IBuildDefinition> buildDefinitions)
        {
            BuildDefinitions.AddRange(buildDefinitions.Select(buildDefinition =>
            {
                var vm = new BuildDefinitionViewModel(buildDefinition);

                var definition = MonitorSettings.SelectedBuildDefinitions.SingleOrDefault(a => a.Id == vm.BuildDefinition.Id);
                if (definition != null && this.buildDefinitionEqualityComparer.Equals(definition, vm.BuildDefinition))
                {
                    vm.IsSelected = true;
                }

                return vm;
            }));

            var obsoleteDefinitions = MonitorSettings.SelectedBuildDefinitions.Where(a => BuildDefinitions.All(x => x.BuildDefinition.Id != a.Id)).ToList();
            foreach (var obsoleteDefinition in obsoleteDefinitions)
            {
                var vm = new BuildDefinitionViewModel(obsoleteDefinition)
                {
                    IsDeleted = true,
                    IsSelected = true
                };
                BuildDefinitions.Add(vm);
            }
        }

        private void InitializeCommands()
        {
            ConnectCommand = new DelegateCommand(() => Task.Run(OnConnectAsync), OnCanConnect);
        }

        private Boolean OnCanConnect()
        {
            return !IsConnecting && !String.IsNullOrWhiteSpace(ProjectUrl) && UrlValidator.IsValidUrl(ProjectUrl);
        }

        private async Task OnConnectAsync()
        {
            IsConnecting = true;

            try
            {
                Application.Current.Dispatcher.Invoke(() => { BuildDefinitions.Clear(); });

                var buildDefinitions = await BuildProvider.GetBuildDefinitionsAsync(MonitorSettings).ConfigureAwait(false);

                if (buildDefinitions.IsSuccessStatusCode)
                {
                    Application.Current.Dispatcher.Invoke(() => { ApplyBuildDefinitions(buildDefinitions.Data); });
                    StatusText = String.Empty;
                    ShowPersonalAccessTokenInput = false;
                }
                else
                {
                    AccessTokenInputWhenUnauthorized(buildDefinitions.StatusCode);
                }
            }
            catch (AdvancedHttpRequestException ex)
            {
                AccessTokenInputWhenUnauthorized(ex.StatusCode);
            }
            catch (Exception ex)
            {
                StatusText = ex.Message;
            }

            IsConnecting = false;
        }

        private void AccessTokenInputWhenUnauthorized(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                ShowPersonalAccessTokenInput = true;
                StatusText = $"Authorization failed, please provide personal access token!";
            }
            else
            {
                StatusText = $"Error connecting. StatusCode: {statusCode}";
            }
        }

        private void SetDisplayName(IEnumerable<IBuildDefinition> buildDefinitions)
        {
            // this is okay as they are from the same project
            var buildDefinition = buildDefinitions.FirstOrDefault();
            if (buildDefinition != null)
            {
                var projectName = buildDefinition.Project.Name;
                DisplayName = $"{ProviderName} - {projectName}";
            }
            else
            {
                DisplayName = ProviderName;
            }
        }

        private void UpdateSelectedCollection()
        {
            this.selectedBuildDefinitions.Clear();
            this.selectedBuildDefinitions.AddRange(MonitorSettings.SelectedBuildDefinitions);
        }

        private readonly IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer;

        private String displayName;

        private Boolean isConnecting;
        private readonly RangeObservableCollection<IBuildDefinition> selectedBuildDefinitions = new RangeObservableCollection<IBuildDefinition>();

        private Boolean showPersonalAccessTokenInput;

        private String statusText;
    }
}