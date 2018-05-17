using System;
using System.Threading.Tasks;
using BuildsAppReborn.Access.Base;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal abstract class TfsUser : NamedObject, IUser
    {
        [JsonProperty("displayName")]
        public String DisplayName { get; private set; }

        [JsonProperty("id")]
        public String Id { get; private set; }

        [JsonIgnore]
        public Byte[] ImageData
        {
            get
            {
                if (this.imageData == null && ImageDataLoader != null)
                {
                    this.imageData = Task.Run(() => ImageDataLoader).Result;
                }

                return this.imageData;
            }
        }

        [JsonProperty("imageUrl")]
        public String ImageUrl { get; private set; }

        public override String Name
        {
            get { return base.Name ?? DisplayName; }
            protected set { base.Name = value; }
        }

        [JsonProperty("uniqueName")]
        public String UniqueName { get; private set; }

        [JsonProperty("url")]
        public String Url { get; private set; }

        [JsonIgnore]
        internal Task<Byte[]> ImageDataLoader { get; set; }

        private Byte[] imageData;
    }
}