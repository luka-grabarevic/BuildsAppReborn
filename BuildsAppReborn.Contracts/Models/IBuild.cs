using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IBuild
    {
        #region Public Properties

        IBuildDefinition Definition { get; }

        DateTime FinishDateTime { get; }

        Int32 Id { get; }

        DateTime QueueDateTime { get; }

        IUser Requester { get; }

        DateTime StartDateTime { get; }

        BuildStatus Status { get; }

        String Url { get; }

        String PortalUrl { get; }

        #endregion
    }
}