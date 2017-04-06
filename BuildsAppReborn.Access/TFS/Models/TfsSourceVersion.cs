﻿using System;
using BuildsAppReborn.Access.Models.Internal;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace BuildsAppReborn.Access.Models
{
    internal class TfsSourceVersion : LinksContainer, ISourceVersion
    {
        #region Implementation of ISourceVersion

        [JsonProperty("comment")]
        public String Comment { get; private set; }

        [JsonIgnore]
        public String PortalUrl => WebLink;

        #endregion
    }
}