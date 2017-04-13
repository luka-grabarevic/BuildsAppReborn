using System;

namespace BuildsAppReborn.Contracts.Composition
{
    public interface IPriorityMetadata
    {
        #region Public Properties

        Int32 Priority { get; }

        #endregion
    }
}