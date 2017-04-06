using System;

using BuildsAppReborn.Contracts.Models;

using Newtonsoft.Json;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access
{
    internal class GitHubSourceVersion : ISourceVersion
    {
        #region Implementation of ISourceVersion

        [JsonIgnore]
        public INamedObject Author => this.commit?.Author;

        [JsonIgnore]
        public String Comment => this.commit?.Message;

        [JsonProperty("html_url")]
        public String PortalUrl { get; private set; }

        #endregion

        #region Private Fields

        [JsonProperty("commit")]
        private GitHubCommit commit;

        #endregion
    }
}