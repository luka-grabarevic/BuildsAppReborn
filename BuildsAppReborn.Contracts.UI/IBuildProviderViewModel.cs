using System;
using System.Collections.Generic;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderViewModel
    {
        String DisplayName { get; }

        BuildMonitorSettings MonitorSettings { get; }

        IEnumerable<IBuildDefinition> SelectedBuildDefinitions { get; }

        String Url { get; }

        Boolean IsInEditMode { get; set; }

        void Initialize(BuildMonitorSettings settings);
    }
}