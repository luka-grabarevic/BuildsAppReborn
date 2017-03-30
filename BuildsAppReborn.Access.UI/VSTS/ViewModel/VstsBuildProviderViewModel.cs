using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Access.UI.ViewModel
{
    [IdentifierExport(typeof(VstsBuildProviderViewModel), VstsBuildProvider.Id)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class VstsBuildProviderViewModel : TfsBuildProviderViewModelBase
    {
        #region Constructors

        [ImportingConstructor]
        public VstsBuildProviderViewModel(IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer)
            : base(buildDefinitionEqualityComparer)
        {
        }

        #endregion

        #region Overrides of Base

        protected override String ProviderName => VstsBuildProvider.Name;

        #endregion
    }
}