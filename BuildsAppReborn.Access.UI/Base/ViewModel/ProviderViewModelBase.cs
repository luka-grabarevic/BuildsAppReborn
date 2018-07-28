using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Access.UI.ViewModel
{
    internal abstract class ProviderViewModelBase : ViewModelBase, IBuildProviderViewModel, IPartImportsSatisfiedNotification
    {
        public abstract String DisplayName { get; protected set; }

        public BuildMonitorSettings MonitorSettings { get; private set; }

        public abstract IEnumerable<IBuildDefinition> SelectedBuildDefinitions { get; }

        public Boolean SupportsDefaultCredentials => BuildProviderMetaData.SupportedAuthenticationModes.HasFlag(AuthenticationModes.Default);

        public Boolean SupportsPersonalAccessToken => BuildProviderMetaData.SupportedAuthenticationModes.HasFlag(AuthenticationModes.AccessToken);

        public abstract String Url { get; }

        public Boolean IsInEditMode
        {
            get { return this.isInEditMode; }
            set
            {
                this.isInEditMode = value;
                OnPropertyChanged();
            }
        }

        public virtual void Initialize(BuildMonitorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            MonitorSettings = settings;
            OnInitialized();
        }

        public virtual void OnImportsSatisfied()
        {
            var attributeType = typeof(IdentifierExportAttribute);
            var customAttributes = GetType().GetCustomAttributes(attributeType, true);
            if (!customAttributes.Any())
            {
                throw new Exception($"No attribute set of type {attributeType.FullName}!");
            }

            var identifierExportAttribute = customAttributes.Single() as IdentifierExportAttribute;
            var result = this.buildProviders.SingleOrDefault(metadata => metadata.Metadata.Id == identifierExportAttribute?.Id);
            if (result != null)
            {
                BuildProvider = result.Value;
                BuildProviderMetaData = result.Metadata;
            }
            else
            {
                throw new Exception($"No build provider found for id {identifierExportAttribute?.Id}");
            }
        }

        protected IBuildProvider BuildProvider { get; private set; }

        protected IBuildProviderMetadata BuildProviderMetaData { get; private set; }

        protected virtual void OnInitialized()
        {
        }

        [ImportMany]
#pragma warning disable 649
        private Lazy<IBuildProvider, IBuildProviderMetadata>[] buildProviders;

        private Boolean isInEditMode;

#pragma warning restore 649
    }
}