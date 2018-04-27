using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal abstract class TfsPullRequest : IPullRequest
    {
        #region Implementation of IPullRequest

        [JsonProperty("createdBy")]
        public virtual IUser CreatedBy { get; protected set; }

        [JsonProperty("description")]
        public String Description { get; private set; }

        [JsonProperty("pullRequestId")]
        public Int32 Id { get; private set; }

        [JsonProperty("mergeStatus")]
        public String MergeStatus { get; private set; }

        [JsonProperty("sourceRefName")]
        public String Source { get; private  set; }

        [JsonProperty("status")]
        public String Status { get; private set; }

        [JsonProperty("targetRefName")]
        public String Target { get; private set; }

        [JsonProperty("title")]
        public String Title { get; private set; }

        #endregion
    }
}