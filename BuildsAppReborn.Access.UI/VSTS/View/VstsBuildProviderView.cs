using System.ComponentModel.Composition;
using BuildsAppReborn.Access.UI.ViewModel;
using BuildsAppReborn.Access.UI.Views;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Access.UI.View
{
    [IdentifierExport(typeof(IBuildProviderView), VstsBuildProvider.Id, VstsBuildProvider.Name)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class VstsBuildProviderView : TfsBuildProviderViewBase
    {
        [Import(typeof(VstsBuildProviderViewModel))]
        public override IBuildProviderViewModel ViewModel
        {
            get { return base.ViewModel; }
            protected set { base.ViewModel = value; }
        }
    }
}