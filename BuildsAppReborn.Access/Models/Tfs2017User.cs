using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Tfs2017User : IUser
    {
        #region Implementation of IUser

        [JsonProperty("displayName")]
        public String DisplayName { get; private set; }

        [JsonProperty("id")]
        public String Id { get; private set; }

        [JsonProperty("imageUrl")]
        public String ImageUrl { get; private set; }

        [JsonProperty("uniqueName")]
        public String UniqueName { get; private set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        #endregion
    }
}