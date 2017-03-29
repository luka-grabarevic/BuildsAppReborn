using System.Windows.Controls;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Access.UI
{
    internal abstract class BuildProviderViewBase : UserControl, IBuildProviderView
    {
        #region Implementation of IBuildProviderView

        public virtual IBuildProviderViewModel ViewModel
        {
            get { return (IBuildProviderViewModel) DataContext; }
            protected set { this.DataContext = value; }
        }

        #endregion
    }
}