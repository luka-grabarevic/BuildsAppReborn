using System;

namespace BuildsAppReborn.Client.Interfaces
{
    public interface ISubSettingsControl : IHasDataContext
    {
        UInt32 Order { get; }

        String Title { get; }
    }
}