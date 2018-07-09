using System;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IUser : IObjectItem
    {
        String DisplayName { get; }

        String Id { get; }

        Byte[] ImageData { get; }

        String ImageUrl { get; }

        String UniqueName { get; }
    }
}