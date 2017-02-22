using System;
using System.ComponentModel.Composition;

using BuildsAppReborn.Access.UI.ViewModel;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Access.UI.Views
{
    [BuildProviderExport(typeof(IBuildProviderView), Tfs2017BuildProvider.Id, Tfs2017BuildProvider.Name)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal partial class Tfs2017BuildProviderView
    {
        #region Constructors

        [ImportingConstructor]
        internal Tfs2017BuildProviderView(Tfs2017BuildProviderViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides of Base

        public override String DisplayName => Tfs2017BuildProvider.Name;

        #endregion
    }
}