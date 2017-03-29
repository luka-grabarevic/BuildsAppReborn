using System.Collections.Generic;
using System.Threading.Tasks;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts
{
    public interface IBuildProvider
    {
        #region Public Methods

        Task<DataResponse<IEnumerable<IBuildDefinition>>> GetBuildDefinitions(BuildMonitorSettings settings);

        Task<DataResponse<IEnumerable<IBuild>>> GetBuilds(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings);

        #endregion
    }
}