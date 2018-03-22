using System;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IUser : IObjectItem
    {
        #region Public Properties

        String DisplayName { get; }

        String Id { get; }

        Byte[] ImageData { get; }

        String ImageUrl { get; }

        String UniqueName { get; }

        #endregion
    }
}