using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface ISourceVersion
    {
        #region Public Properties

        INamedObject Author { get; }

        String Comment { get; }

        String PortalUrl { get; }

        #endregion
    }
}