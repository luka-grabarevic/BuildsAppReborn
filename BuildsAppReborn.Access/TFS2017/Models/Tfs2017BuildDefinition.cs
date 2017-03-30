using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Tfs2017BuildDefinition : TfsBuildDefinition
    {
        #region Overrides of Base

        [JsonConverter(typeof(InterfaceTypeConverter<Tfs2017Project, IProject>))]
        public override IProject Project { get; protected set; }

        #endregion
    }
}