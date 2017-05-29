using System;
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
        #region Implementation of IBuild

        [JsonProperty("buildNumber")]
        public String BuildNumber { get; private set; }

        [JsonProperty("definition")]
        public abstract IBuildDefinition Definition { get; protected set; }

        [JsonProperty("id")]
        public Int32 Id { get; private set; }

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

        [JsonProperty("finishTime")]
        public DateTime FinishDateTime { get; private set; }

        [JsonProperty("queueTime")]
        public DateTime QueueDateTime { get; private set; }

        [JsonProperty("startTime")]
        public DateTime StartDateTime { get; private set; }

        [JsonProperty("requestedFor")]
        public abstract IUser Requester { get; protected set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        [JsonIgnore]
        public String PortalUrl => base.WebLink;

        [JsonIgnore]
        public ISourceVersion SourceVersion { get; internal set; }

        #endregion

        #region Internal Properties

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

        #endregion

        #region Private Fields

        [JsonProperty("result")]
        private String result;

        private String sourceVersionInternal;

        [JsonProperty("status")]
        private String status;

        #endregion
    }
}