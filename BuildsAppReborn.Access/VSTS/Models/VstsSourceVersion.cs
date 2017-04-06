using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal class VstsSourceVersion : TfsSourceVersion
    {
        #region Overrides of Base

        [JsonConverter(typeof(InterfaceTypeConverter<VstsUser, INamedObject>))]
        public override INamedObject Author { get; protected set; }

        #endregion
    }
}