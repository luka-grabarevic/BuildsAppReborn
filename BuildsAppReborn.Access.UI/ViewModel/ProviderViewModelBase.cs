using System;

using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Access.UI.ViewModel
{
    internal class ProviderViewModelBase : ViewModelBase, IBuildProviderViewModel
    {
        #region Implementation of IBuildProviderViewModel

        public BuildMonitorSettings MonitorSettings { get; private set; }

        public virtual void Initialize(BuildMonitorSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            MonitorSettings = settings;
            OnInitialized();
        }

        #endregion

        #region Protected Methods

        protected virtual void OnInitialized()
        {
        }

        #endregion

        protected internal IBuildProvider BuildProvider { get; set; }
    }
}