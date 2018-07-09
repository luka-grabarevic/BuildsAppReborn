using System;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models.Internal
{
    internal class Repository
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonConverter(typeof(TolerantEnumConverter))]
        [JsonProperty("type")]
        public RepositoryType RepositoryType { get; set; }
    }
}