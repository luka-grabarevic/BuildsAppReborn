using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal class VstsSourceVersion : TfsSourceVersion
    {
        [JsonConverter(typeof(InterfaceTypeConverter<VstsUser, INamedObject>))]
        public override INamedObject Author { get; protected set; }
    }
}