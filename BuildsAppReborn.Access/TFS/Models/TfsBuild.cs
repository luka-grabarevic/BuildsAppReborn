using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

#pragma warning disable 649

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal abstract class TfsBuild : IBuild
    {
        #region Implementation of IBuild

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
        public String PortalUrl { get; internal set; }

        #endregion

        #region Private Fields

        [JsonProperty("result")]
        private String result;

        [JsonProperty("status")]
        private String status;

        #endregion
    }
}