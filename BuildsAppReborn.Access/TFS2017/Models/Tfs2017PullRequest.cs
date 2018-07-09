using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    // ReSharper disable once UnusedMember.Global
    internal class Tfs2017PullRequest : TfsPullRequest
    {
        [JsonConverter(typeof(InterfaceTypeConverter<Tfs2017User, IUser>))]
        public override IUser CreatedBy { get; protected set; }
    }
}