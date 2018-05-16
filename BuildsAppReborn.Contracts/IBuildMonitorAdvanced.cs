using System;
using System.Collections.Generic;

using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts
{
    /// <summary>
    /// monitors builds from a single settings object
    /// </summary>
    public interface IBuildMonitorAdvanced : IBuildMonitorBasic
    {
        #region Public Methods

        void Start(IEnumerable<BuildMonitorSettings> settings, GeneralSettings generalSettings, TimeSpan pollingInterval);

        void Stop();

        #endregion
    }
}