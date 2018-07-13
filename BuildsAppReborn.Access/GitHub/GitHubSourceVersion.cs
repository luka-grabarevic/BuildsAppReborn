using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access
{
    internal class GitHubSourceVersion : ISourceVersion
    {
        [JsonIgnore]
        public INamedObject Author => this.commit?.Author;

        [JsonIgnore]
        public INamedObject Committer { get; set; }

        [JsonIgnore]
        public IUser Pusher { get; set; }

        [JsonIgnore]
        public String Comment => this.commit?.Message;

        [JsonProperty("html_url")]
        public String PortalUrl { get; private set; }

        [JsonProperty("commit")] private GitHubCommit commit;
    }
}