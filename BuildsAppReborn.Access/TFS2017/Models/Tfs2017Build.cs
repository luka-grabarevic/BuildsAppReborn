using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal class Tfs2017Build : TfsBuild
    {
        #region Overrides of Base

        [JsonConverter(typeof(InterfaceTypeConverter<Tfs2017BuildDefinition, IBuildDefinition>))]
        public override IBuildDefinition Definition { get; protected set; }

        [JsonConverter(typeof(InterfaceTypeConverter<Tfs2017User, IUser>))]
        public override IUser Requester { get; protected set; }

        #endregion
    }
}