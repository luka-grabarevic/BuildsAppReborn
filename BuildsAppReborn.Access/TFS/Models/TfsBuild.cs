using System;
using System.Collections.Generic;
using System.Linq;
using BuildsAppReborn.Access.Models.Internal;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

#pragma warning disable 649

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal abstract class TfsBuild : LinksContainer, IBuild
    {
        public TfsBuild()
        {
            Artifacts = Enumerable.Empty<IArtifact>();
        }

        [JsonIgnore]
        public IEnumerable<IArtifact> Artifacts { get; internal set; }

        [JsonProperty("buildNumber")]
        public String BuildNumber { get; private set; }

        [JsonProperty("definition")]
        public abstract IBuildDefinition Definition { get; protected set; }

        [JsonIgnore]
        public IUser DisplayUser
        {
            get
            {
                // if not requested by service user, it was by user. So show user.
                if (Requester != null && !Requester.IsServiceUser)
                {
                    return Requester;
                }

                // tries to determine the pusher of the commit, and show him if not service user
                if (SourceVersion != null)
                {
                    if (!SourceVersion.Pusher.IsServiceUser)
                    {
                        return SourceVersion.Pusher;
                    }
                }

                // fallback value
                return Requester;
            }
        }

        [JsonProperty("finishTime")]
        public DateTime FinishDateTime { get; private set; }

        [JsonProperty("id")]
        public Int32 Id { get; private set; }

        [JsonIgnore]
        public IPullRequest PullRequest { get; internal set; }

        [JsonProperty("queueTime")]
        public DateTime QueueDateTime { get; private set; }

        [JsonIgnore]
        public BuildReason Reason
        {
            get
            {
                if (this.reason == "batchedCI" ||
                    this.reason == "individualCI" ||
                    this.reason == "checkInShelveset")
                {
                    return BuildReason.ContinuousIntegration;
                }

                if (this.reason == "manual")
                {
                    return BuildReason.Manual;
                }

                if (this.reason == "pullRequest")
                {
                    return BuildReason.PullRequest;
                }

                if (this.reason == "schedule")
                {
                    return BuildReason.Schedule;
                }

                if (this.reason == "triggered")
                {
                    return BuildReason.Triggered;
                }

                if (this.reason == "validateShelveset" ||
                    this.reason == "checkInShelveset")
                {
                    return BuildReason.Validation;
                }

                return BuildReason.Unknown;
            }
        }

        [JsonProperty("requestedBy")]
        public abstract IUser Requester { get; protected set; }

        [JsonIgnore]
        public ISourceVersion SourceVersion { get; internal set; }

        [JsonProperty("startTime")]
        public DateTime StartDateTime { get; private set; }

        [JsonIgnore]
        public BuildStatus Status
        {
            /*
             * TFS:
             * result: enum { succeeded, partiallySucceeded, failed, canceled } 
             * status: enum { inProgress, completed, cancelling, postponed, notStarted, all } 
             * 
             */
            get
            {
                if (this.status == "inProgress")
                {
                    return BuildStatus.Running;
                }

                if (this.status == "completed" && this.result == "succeeded")
                {
                    return BuildStatus.Succeeded;
                }

                if (this.status == "completed" && this.result == "failed")
                {
                    return BuildStatus.Failed;
                }

                if (this.status == "completed" && this.result == "partiallySucceeded")
                {
                    return BuildStatus.PartiallySucceeded;
                }

                if (this.status == "cancelling" || this.result == "canceled")
                {
                    return BuildStatus.Stopped;
                }

                if (this.status == "notStarted")
                {
                    return BuildStatus.Queued;
                }

                return BuildStatus.Unknown;
            }
        }

        [JsonIgnore]
        public IEnumerable<ITestRun> TestRuns { get; internal set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        [JsonIgnore]
        public String WebUrl => WebLink;

        [JsonProperty("repository")]
        internal Repository Repository { get; private set; }

        [JsonProperty("sourceBranch")]
        internal String SourceBranch { get; private set; }

        [JsonProperty("sourceVersion")]
        internal String SourceVersionInternal
        {
            get
            {
                // when it was a gated check in somehow the source version contains a C at the beginning
                return this.sourceVersionInternal.TrimStart('C');
            }
            private set { this.sourceVersionInternal = value; }
        }

        [JsonProperty("uri")]
        internal String Uri { get; private set; }

        [JsonProperty("reason")] private String reason;

        [JsonProperty("result")] private String result;

        private String sourceVersionInternal;

        [JsonProperty("status")] private String status;
    }
}