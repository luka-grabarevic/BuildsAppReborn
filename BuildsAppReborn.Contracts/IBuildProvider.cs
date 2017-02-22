using System.Collections.Generic;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts
{
    public interface IBuildProvider
    {
        #region Public Methods

        IEnumerable<IBuildDefinition> GetBuildDefinitions(BuildMonitorSettings settings);

        IEnumerable<IBuild> GetBuilds(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings);

        #endregion
    }
}