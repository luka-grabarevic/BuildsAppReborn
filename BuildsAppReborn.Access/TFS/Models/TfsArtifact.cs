using System;
using BuildsAppReborn.Access.Models.Internal;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal abstract class TfsArtifact : IArtifact
    {
        [JsonIgnore]
        public String Data => this.resource?.Data;

        [JsonIgnore]
        public String DownloadUrl => this.resource?.DownloadUrl;

        [JsonProperty("id")]
        public Int32 Id { get; protected set; }

        [JsonProperty("name")]
        public String Name { get; protected set; }

        public String Type => this.resource?.Type;

        [JsonProperty("resource")] private Resource resource;
    }
}