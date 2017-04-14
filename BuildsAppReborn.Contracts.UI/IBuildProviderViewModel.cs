using System;
using System.Collections.Generic;

using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderViewModel
    {
        #region Public Properties

        String DisplayName { get; }

        BuildMonitorSettings MonitorSettings { get; }

        IEnumerable<IBuildDefinition> SelectedBuildDefinitions { get; }

        String Url { get; }

        #endregion

        #region Public Methods

        void Initialize(BuildMonitorSettings settings);

        #endregion
    }
}