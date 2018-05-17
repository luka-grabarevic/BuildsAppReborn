using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface ISourceVersion
    {
        INamedObject Author { get; }

        String Comment { get; }

        String PortalUrl { get; }
    }
}