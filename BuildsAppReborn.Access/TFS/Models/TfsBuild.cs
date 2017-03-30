using System;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

#pragma warning disable 649

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class TfsBuild : IBuild
    {
        #region Implementation of IBuild

        [JsonProperty("definition")]
        [JsonConverter(typeof(InterfaceTypeConverter<TfsBuildDefinition, IBuildDefinition>))]
        public IBuildDefinition Definition { get; private set; }

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
             */ get
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

        #endregion

        #region Private Fields

        [JsonProperty("result")]
        private String result;

        [JsonProperty("status")]
        private String status;

        [JsonProperty("requestedFor")]
        [JsonConverter(typeof(InterfaceTypeConverter<TfsUser, IUser>))]
        public IUser Requester { get; private set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        #endregion
    }
}