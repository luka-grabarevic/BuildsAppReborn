using System;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class TfsBuildDefinition : IBuildDefinition
    {
        #region Implementation of IBuildDefinition

        [JsonProperty("id")]
        public Int32 Id { get; private set; }

        [JsonProperty("name")]
        public String Name { get; private set; }

        [JsonProperty("type")]
        public String Type { get; private set; }

        [JsonProperty("project")]
        [JsonConverter(typeof(InterfaceTypeConverter<TfsProject, IProject>))]
        public IProject Project { get; private set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        [JsonIgnore]
        public String BuildSettingsId { get; internal set; }

        #endregion
    }
}