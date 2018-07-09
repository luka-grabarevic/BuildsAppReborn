using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IArtifact
    {
        String Data { get; }

        String DownloadUrl { get; }

        Int32 Id { get; }

        String Name { get; }

        String Type { get; }
    }
}