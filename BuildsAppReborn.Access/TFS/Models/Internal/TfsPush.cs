using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models.Internal
{
    internal class TfsPush<TUser> : ITfsPush where TUser : IUser
    {
        [JsonIgnore]
        public IUser PushUser => InternalUser;

        [JsonProperty("pushedBy")]
        private TUser InternalUser { get; set; }
    }
}