using System;

namespace BuildsAppReborn.Contracts
{
    public interface IProgressComponent
    {
        Double MaximumProgress { get; }

        event PollingProgressUpdated ProgressUpdated;
    }

    public delegate void PollingProgressUpdated(Double progress);
}