using System;
using BuildsAppReborn.Access.Models.Internal;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace BuildsAppReborn.Access.Models
{
    internal abstract class TfsSourceVersion : LinksContainer, ISourceVersion
    {
        [JsonProperty("author")]
        public abstract INamedObject Author { get; protected set; }

        [JsonProperty("comment")]
        public String Comment { get; private set; }

        [JsonIgnore]
        public String PortalUrl => WebLink;
    }
}