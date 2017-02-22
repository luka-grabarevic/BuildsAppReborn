using System;

namespace BuildsAppReborn.Contracts.Composition
{
    public interface IBuildProviderMetadata
    {
        #region Public Properties

        String Id { get; }

        String Name { get; }

        #endregion
    }
}