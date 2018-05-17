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
        [ImportingConstructor]
        public VstsBuildProviderViewModel(IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer)
            : base(buildDefinitionEqualityComparer)
        {
        }

        protected override String ProviderName => VstsBuildProvider.Name;
    }
}