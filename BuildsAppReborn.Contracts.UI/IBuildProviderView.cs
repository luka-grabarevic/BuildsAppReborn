using System;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderView
    {
        #region Public Properties

        String DisplayName { get; }

        IBuildProviderViewModel ViewModel { get; }

        #endregion
    }
}