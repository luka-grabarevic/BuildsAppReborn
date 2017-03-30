using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public abstract class TfsProject : IProject
    {
        #region Implementation of IProject

        [JsonProperty("description")]
        public String Description { get; private set; }

        [JsonProperty("id")]
        public String Id { get; private set; }

        [JsonProperty("name")]
        public String Name { get; private set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        #endregion
    }
}