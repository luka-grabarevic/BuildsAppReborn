using System;
using System.ComponentModel;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models.Internal
{
    internal class Repository
    {
        #region Public Properties

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonConverter(typeof(TolerantEnumConverter))]
        [JsonProperty("type")]
        public RepositoryType RepositoryType { get; set; }

        #endregion
    }
}