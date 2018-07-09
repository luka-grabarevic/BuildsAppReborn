using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Base
{
    public class NamedObject : INamedObject
    {
        [JsonProperty("name")]
        public virtual String Name { get; protected set; }
    }
}