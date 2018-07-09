using System.Collections.Generic;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts
{
    /// <summary>
    /// monitors builds from a single settings object
    /// </summary>
    public interface IBuildMonitorAdvanced : IBuildMonitorBasic
    {
        void Start(IEnumerable<BuildMonitorSettings> settings, GeneralSettings generalSettings);

        void Stop();
    }
}