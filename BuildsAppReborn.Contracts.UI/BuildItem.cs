using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        public IBuild Build { get; }

        public TimeSpan BuildDuration => BuildEndTime - BuildStartTime;

        public DateTime BuildEndTime
        {
            get
            {
                switch (Build.Status)
                {
                    case BuildStatus.Queued:
                    case BuildStatus.Running:
                    case BuildStatus.Unknown:
                        return DateTime.UtcNow;
                    default:
                        return Build.FinishDateTime;
                }
            }
        }

        public DateTime BuildStartTime
        {
            get
            {
                switch (Build.Status)
                {
                    case BuildStatus.Queued:
                    case BuildStatus.Unknown:
                        return Build.QueueDateTime;
                    default:
                        return Build.StartDateTime;
                }
            }
        }

        public DateTime BuildStateTime
        {
            get
            {
                switch (Build.Status)
                {
                    case BuildStatus.Unknown:
                        return Build.QueueDateTime;
                    case BuildStatus.Succeeded:
                    case BuildStatus.PartiallySucceeded:
                    case BuildStatus.Failed:
                    case BuildStatus.Stopped:
                        return Build.FinishDateTime;
                    case BuildStatus.Running:
                        return Build.StartDateTime;
                    case BuildStatus.Queued:
                    default:
                        return Build.QueueDateTime;
                }
            }
        }

        public BuildStatus BuildStatus => Build?.Status ?? BuildStatus.Unknown;

        public String Comment
        {
            get
            {
                if (Build?.SourceVersion?.Comment != null)
                {
                    return !RequesterIsCommitter ? $"{Build.SourceVersion.Author.Name}: {Build.SourceVersion.Comment}" : Build.SourceVersion.Comment;
                }

                return "-";
            }
        }

        public ITestRun CurrentTestRun => Build?.TestRuns?.FirstOrDefault();

        public String Description => $"{Build.Definition.Name} - {Build.BuildNumber}";

        public Byte[] RequesterImage => Build?.Requester?.ImageData;

        public String RequesterText => !RequesterIsCommitter ? $"Requested by: {Build.Requester.DisplayName}" : Build.Requester.DisplayName;

        #endregion

        #region Public Methods

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void Refresh()
        {
            OnPropertyChanged(nameof(BuildStartTime));
            OnPropertyChanged(nameof(BuildEndTime));
            OnPropertyChanged(nameof(BuildStateTime));
            OnPropertyChanged(nameof(BuildDuration));
            OnPropertyChanged(nameof(BuildStatus));
            OnPropertyChanged(nameof(CurrentTestRun));
        }

        #endregion

        #region Private Properties

        private Boolean RequesterIsCommitter => Build?.SourceVersion?.Author?.Name == Build?.Requester?.DisplayName;

        #endregion
    }
}