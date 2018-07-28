using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderViewModel
    {
        String DisplayName { get; }

        Boolean IsInEditMode { get; }

        BuildMonitorSettings MonitorSettings { get; }

        ObservableCollection<IBuildDefinition> SelectedBuildDefinitions { get; }

        String Url { get; }

        void Initialize(BuildMonitorSettings settings);

        void Save();
    }
}