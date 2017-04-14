using System;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderView
    {
        #region Public Properties

        String Header { get; }

        IBuildProviderViewModel ViewModel { get; }

        #endregion
    }
}