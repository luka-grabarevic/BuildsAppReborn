namespace BuildsAppReborn.Contracts.Models
{
    public enum BuildReason
    {
        Unknown,
        ContinuousIntegration,
        Manual,
        PullRequest,
        Schedule,
        Triggered,
        Validation
    }
}