using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Contracts.UI
{
    public class BuildItem : ViewModelBase
    {
        public BuildItem(IBuild build, BuildViewStyle viewStyle)
        {
            this.viewStyle = viewStyle;
            Build = build;
        }

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
                    return ManualRequested ? $"{Build.DisplayUser.DisplayName}: {Build.SourceVersion.Comment}" : Build.SourceVersion.Comment;
                }

                return "-";
            }
        }

        public ITestRun CurrentTestRun => Build?.TestRuns?.FirstOrDefault();

        public String Description
        {
            get
            {
                if (this.viewStyle == BuildViewStyle.GroupByPullRequest)
                {
                    return $"{Build.PullRequest.Title} - {Build.BuildNumber}";
                }

                return $"{Build.Definition.Name} - {Build.BuildNumber}";
            }
        }

        public Byte[] DisplayUserImage => Build?.DisplayUser?.ImageData;

        public String DisplayUserText => ManualRequested ? $"Requested by: {Build.Requester.DisplayName}" : Build.DisplayUser.DisplayName;

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

        private Boolean ManualRequested => Build.Reason == BuildReason.Manual;

        private readonly BuildViewStyle viewStyle;
    }
}