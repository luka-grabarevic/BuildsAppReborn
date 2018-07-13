using System;
using System.Collections.Generic;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IBuild : IObjectItem, IWebItem
    {
        IEnumerable<IArtifact> Artifacts { get; }

        String BuildNumber { get; }

        IBuildDefinition Definition { get; }

        IUser DisplayUser { get; }

        DateTime FinishDateTime { get; }

        Int32 Id { get; }

        IPullRequest PullRequest { get; }

        DateTime QueueDateTime { get; }

        BuildReason Reason { get; }

        IUser Requester { get; }

        ISourceVersion SourceVersion { get; }

        DateTime StartDateTime { get; }

        BuildStatus Status { get; }

        IEnumerable<ITestRun> TestRuns { get; }
    }
}