using System;
using System.Collections.Generic;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IBuild
    {
        #region Public Properties

        IEnumerable<IArtifact> Artifacts { get; }

        String BuildNumber { get; }

        IBuildDefinition Definition { get; }

        DateTime FinishDateTime { get; }

        Int32 Id { get; }

        String PortalUrl { get; }

        DateTime QueueDateTime { get; }

        IUser Requester { get; }

        ISourceVersion SourceVersion { get; }

        DateTime StartDateTime { get; }

        BuildStatus Status { get; }

        String Url { get; }

        #endregion
    }
}