using System.ComponentModel.Composition;

using BuildsAppReborn.Access.UI.ViewModel;
using BuildsAppReborn.Access.UI.Views;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Access.UI.View
{
    [IdentifierExport(typeof(IBuildProviderView), Tfs2017BuildProvider.Id, Tfs2017BuildProvider.Name)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class Tfs2017BuildProviderView : TfsBuildProviderViewBase
    {
        #region Overrides of Base

        [Import(typeof(Tfs2017BuildProviderViewModel))]
        public override IBuildProviderViewModel ViewModel
        {
            get
            {
                return base.ViewModel;
            }
            protected set
            {
                base.ViewModel = value;
            }
        }

        #endregion
    }
}