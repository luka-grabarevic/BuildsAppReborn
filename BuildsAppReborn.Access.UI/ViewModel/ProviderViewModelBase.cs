using System;
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
        #region Implementation of IBuildProviderViewModel

        public BuildMonitorSettings MonitorSettings { get; private set; }

        public virtual void Initialize(BuildMonitorSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            MonitorSettings = settings;
            OnInitialized();
        }

        public abstract String DisplayName { get; protected set; }

        #endregion

        #region Implementation of IPartImportsSatisfiedNotification

        public void OnImportsSatisfied()
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

        #endregion

        #region Public Properties

        public Boolean SupportsDefaultCredentials => BuildProviderMetaData.SupportedAuthenticationModes.HasFlag(AuthenticationModes.Default);

        public Boolean SupportsPersonalAccessToken => BuildProviderMetaData.SupportedAuthenticationModes.HasFlag(AuthenticationModes.AccessToken);

        #endregion

        #region Protected Properties

        protected IBuildProvider BuildProvider { get; private set; }

        protected IBuildProviderMetadata BuildProviderMetaData { get; private set; }

        #endregion

        #region Protected Methods

        protected virtual void OnInitialized()
        {
        }

        #endregion

        #region Private Fields

        [ImportMany]
#pragma warning disable 649
        private Lazy<IBuildProvider, IBuildProviderMetadata>[] buildProviders;
#pragma warning restore 649

        #endregion
    }
}