using BuildsAppReborn.Access.Models.Internal;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal class VstsSourceVersion : TfsSourceVersion
    {
        [JsonConverter(typeof(InterfaceTypeConverter<VstsUser, INamedObject>))]
        public override INamedObject Author { get; protected set; }

        [JsonConverter(typeof(InterfaceTypeConverter<VstsUser, INamedObject>))]
        public override INamedObject Committer { get; protected set; }

        [JsonConverter(typeof(InterfaceTypeConverter<TfsPush<VstsUser>, ITfsPush>))]
        internal override ITfsPush Push { get; set; }
    }
}