using System;

namespace BuildsAppReborn.Client.Interfaces
{
    public interface ISubSettingsControl : IHasDataContext
    {
        #region Public Properties

        UInt32 Order { get; }

        String Title { get; }

        #endregion
    }
}