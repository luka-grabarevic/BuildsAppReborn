using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models.Internal
{
    internal abstract class LinksContainer
    {
        [JsonIgnore]
        public String WebLink
        {
            get
            {
                if (this.links?.ContainsKey(Web) != null)
                {
                    var web = this.links[Web];
                    if (web?.ContainsKey(Href) != null)
                    {
                        return web[Href];
                    }
                }

                return String.Empty;
            }
        }

        [JsonProperty("_links")] private Dictionary<String, Dictionary<String, String>> links;

        private const String Web = "web";

        private const String Href = "href";
    }
}