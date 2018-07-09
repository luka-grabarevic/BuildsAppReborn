using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IPullRequest
    {
        IUser CreatedBy { get; }

        String Description { get; }

        Int32 Id { get; }

        String MergeStatus { get; }

        String Source { get; }

        String Status { get; }

        String Target { get; }

        String Title { get; }
    }
}