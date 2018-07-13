using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface ISourceVersion
    {
        INamedObject Author { get; }

        INamedObject Committer { get; }

        IUser Pusher { get; }

        String Comment { get; }

        String PortalUrl { get; }
    }
}