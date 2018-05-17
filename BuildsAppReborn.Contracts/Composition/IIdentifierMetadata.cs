using System;

namespace BuildsAppReborn.Contracts.Composition
{
    public interface IIdentifierMetadata
    {
        String Id { get; }

        String Name { get; }
    }
}