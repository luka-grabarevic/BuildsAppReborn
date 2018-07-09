using System;
using BuildsAppReborn.Access.Base;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access
{
    internal class GitHubCommit
    {
        [JsonProperty("author")]
        [JsonConverter(typeof(InterfaceTypeConverter<NamedObject, INamedObject>))]
        public INamedObject Author { get; private set; }

        [JsonProperty("message")]
        public String Message { get; private set; }
    }
}