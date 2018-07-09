using System.Collections.Generic;
using System.Threading.Tasks;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts
{
    public interface IBuildProvider
    {
        Task<DataResponse<IEnumerable<IBuildDefinition>>> GetBuildDefinitionsAsync(BuildMonitorSettings settings);

        Task<DataResponse<IEnumerable<IBuild>>> GetBuildsAsync(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings);

        Task<DataResponse<IEnumerable<IBuild>>> GetBuildsByPullRequestsAsync(BuildMonitorSettings settings);
    }
}