using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts
{
    public interface IBuildMonitorBasic
    {
        Boolean IsConfigured { get; }

        Task BeginPollingBuildsAsync();

        event BuildsUpdatedEventHandler BuildsUpdated;

        event EventHandler MonitorStarted;

        event EventHandler MonitorStopped;
    }

    public delegate void BuildsUpdatedEventHandler(ICollection<IBuild> builds);
}