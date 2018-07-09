using System;

namespace BuildsAppReborn.Contracts.UI
{
    public interface IBuildProviderView
    {
        String Header { get; }

        IBuildProviderViewModel ViewModel { get; }
    }
}