using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once UnusedMember.Global
    internal class VstsPullRequest : TfsPullRequest
    {
        #region Overrides of Base

        [JsonConverter(typeof(InterfaceTypeConverter<VstsUser, IUser>))]
        public override IUser CreatedBy { get; protected set; }

        #endregion
    }
}