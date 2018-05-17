using System;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models.Internal
{
    internal class Resource
    {
        [JsonProperty("data")]
        public String Data { get; private set; }

        [JsonProperty("downloadUrl")]
        public String DownloadUrl { get; private set; }

        [JsonProperty("type")]
        public String Type { get; private set; }

        [JsonProperty("url")]
        public String Url { get; private set; }
    }
}