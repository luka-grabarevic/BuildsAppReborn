using System;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IBuildDefinition : IObjectItem
    {
        String BuildSettingsId { get; }

        Int32 Id { get; }

        String Name { get; }

        IProject Project { get; }

        String Type { get; }
    }
}