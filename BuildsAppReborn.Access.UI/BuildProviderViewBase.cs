using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;

using BuildsAppReborn.Access.UI.ViewModel;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Access.UI
{
    internal abstract class BuildProviderViewBase : UserControl, IBuildProviderView, IPartImportsSatisfiedNotification
    {
        #region Constructors

        protected BuildProviderViewBase(ProviderViewModelBase viewModel)
        {
            DataContext = viewModel;
        }

        #endregion

        #region Implementation of IBuildProviderView

        public abstract String DisplayName { get; }

        public IBuildProviderViewModel ViewModel => (IBuildProviderViewModel)DataContext;

        #endregion

        #region Implementation of IPartImportsSatisfiedNotification

        public void OnImportsSatisfied()
        {
            var attributeType = typeof(BuildProviderExportAttribute);
            var customAttributes = GetType().GetCustomAttributes(attributeType, true);
            if (!customAttributes.Any())
            {
                throw new Exception($"No attribute set of type {attributeType.FullName}!");
            }

            var attr = customAttributes.Single() as BuildProviderExportAttribute;

            ViewModelBase.BuildProvider = this.buildProviders.GetSingleOrDefault(metadata => metadata.Id == attr?.Id);
        }

        #endregion

        #region Public Properties

        public virtual BuildMonitorSettings MonitorSettings => null;

        #endregion

        #region Private Properties

        private ProviderViewModelBase ViewModelBase => (ProviderViewModelBase)ViewModel;

        #endregion

        #region Private Fields

        [Import]
        private LazyContainer<IBuildProvider, IBuildProviderMetadata> buildProviders;

        #endregion
    }
}