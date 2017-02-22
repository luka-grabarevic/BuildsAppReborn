using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IBuildDefinition
    {
        #region Public Properties

        Int32 Id { get; }

        String Name { get; }

        IProject Project { get; }

        String Type { get; }

        String Url { get; }

        #endregion
    }
}