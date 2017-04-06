using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface ISourceVersion
    {
        #region Public Properties

        String Comment { get; }

        String PortalUrl { get; }

        #endregion
    }
}