using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal class Tfs2017SourceVersion : TfsSourceVersion
    {
        [JsonConverter(typeof(InterfaceTypeConverter<Tfs2017User, INamedObject>))]
        public override INamedObject Author { get; protected set; }
    }
}