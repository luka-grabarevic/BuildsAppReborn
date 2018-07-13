using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Access.Models.Internal
{
    internal interface ITfsPush
    {
        IUser PushUser { get; }
    }
}