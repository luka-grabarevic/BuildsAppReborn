using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class VstsBuildDefinition : TfsBuildDefinition
    {
        [JsonConverter(typeof(InterfaceTypeConverter<VstsProject, IProject>))]
        public override IProject Project { get; protected set; }
    }
}