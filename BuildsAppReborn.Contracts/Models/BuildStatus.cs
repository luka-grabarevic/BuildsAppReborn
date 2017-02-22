namespace BuildsAppReborn.Contracts.Models
{
    public enum BuildStatus
    {
        Unknown,

        Succeeded,

        Failed,

        PartiallySucceeded,

        Running,

        Stopped,

        Queued
    }
}