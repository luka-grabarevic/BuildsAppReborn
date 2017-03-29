using System;

namespace BuildsAppReborn.Contracts.Composition
{
    public interface IIdentifierMetadata
    {
        #region Public Properties

        String Id { get; }

        String Name { get; }

        #endregion
    }
}