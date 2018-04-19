using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal class VstsBuild : TfsBuild
    {
        #region Overrides of Base

        [JsonConverter(typeof(InterfaceTypeConverter<VstsBuildDefinition, IBuildDefinition>))]
        public override IBuildDefinition Definition { get; protected set; }

        [JsonConverter(typeof(InterfaceTypeConverter<VstsUser, IUser>))]
        public override IUser Requester { get; protected set; }


        #endregion
    }
}