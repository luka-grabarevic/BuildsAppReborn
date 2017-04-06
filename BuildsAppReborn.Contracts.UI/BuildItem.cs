using System;

using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Contracts.UI
{
    public class BuildItem : ViewModelBase
    {
        #region Constructors

        public BuildItem(IBuild build)
        {
            Build = build;
        }

        #endregion

        #region Public Properties

        public IBuild Build { get; private set; }

        public DateTime BuildTime
        {
            get
            {
                return GetBuildTime();
            }
            set
            {
                // hack mc hack hack
                OnPropertyChanged();
            }
        }

        public String Comment => Build?.SourceVersion?.Comment ?? "-";

        #endregion

        #region Private Methods

        private DateTime GetBuildTime()
        {
            var result = DateTime.UtcNow;
            switch (Build.Status)
            {
                case BuildStatus.Unknown:
                    result = Build.QueueDateTime;
                    break;
                case BuildStatus.Succeeded:
                case BuildStatus.PartiallySucceeded:
                case BuildStatus.Failed:
                case BuildStatus.Stopped:
                    result = Build.FinishDateTime;
                    break;
                case BuildStatus.Running:
                    result = Build.StartDateTime;
                    break;
                case BuildStatus.Queued:
                    result = Build.QueueDateTime;
                    break;
            }
            return result;
        }

        #endregion
    }
}