using System;
using System.Collections.Generic;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IBuild : IObjectItem, IWebItem
    {
        #region Public Properties

        IEnumerable<IArtifact> Artifacts { get; }

        String BuildNumber { get; }

        IBuildDefinition Definition { get; }

        DateTime FinishDateTime { get; }

        Int32 Id { get; }

        DateTime QueueDateTime { get; }

        IUser Requester { get; }

        ISourceVersion SourceVersion { get; }

        DateTime StartDateTime { get; }

        BuildStatus Status { get; }

        IEnumerable<ITestRun> TestRuns { get; }

        #endregion
    }
}