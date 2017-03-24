using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderViewModel
    {
        #region Public Properties

        String DisplayName { get; }

        BuildMonitorSettings MonitorSettings { get; }

        #endregion

        #region Public Methods

        void Initialize(BuildMonitorSettings settings);

        #endregion
    }
}