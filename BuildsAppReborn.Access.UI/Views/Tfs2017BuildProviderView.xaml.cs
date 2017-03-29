using System.ComponentModel.Composition;
using BuildsAppReborn.Access.UI.ViewModel;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Access.UI.Views
{
    [IdentifierExport(typeof(IBuildProviderView), Tfs2017BuildProvider.Id, Tfs2017BuildProvider.Name)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal partial class Tfs2017BuildProviderView
    {
        #region Constructors

        [ImportingConstructor]
        internal Tfs2017BuildProviderView()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides of Base

        [Import(typeof(Tfs2017BuildProviderViewModel))]
        public override IBuildProviderViewModel ViewModel
        {
            get { return base.ViewModel; }
            protected set { base.ViewModel = value; }
        }

        #endregion
    }
}