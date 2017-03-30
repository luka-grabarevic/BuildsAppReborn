using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Access.UI.ViewModel
{
    [IdentifierExport(typeof(Tfs2017BuildProviderViewModel), Tfs2017BuildProvider.Id)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class Tfs2017BuildProviderViewModel : TfsBuildProviderViewModelBase
    {
        #region Constructors

        [ImportingConstructor]
        public Tfs2017BuildProviderViewModel(IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer)
            : base(buildDefinitionEqualityComparer)
        {
        }

        #endregion

        #region Overrides of Base

        protected override String ProviderName => Tfs2017BuildProvider.Name;

        #endregion
    }
}