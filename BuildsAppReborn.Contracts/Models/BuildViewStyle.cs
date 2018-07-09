using System.ComponentModel;

namespace BuildsAppReborn.Contracts.Models
{
    public enum BuildViewStyle
    {
        [Description("Build Definition")] GroupByBuildDefinition,

        [Description("Pull Request")] GroupByPullRequest
    }
}