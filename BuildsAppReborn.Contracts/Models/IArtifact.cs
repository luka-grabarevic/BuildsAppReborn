using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IArtifact
    {
        #region Public Properties

        String DownloadUrl { get; }

        Int32 Id { get; }

        String Type { get; }

        #endregion
    }
}