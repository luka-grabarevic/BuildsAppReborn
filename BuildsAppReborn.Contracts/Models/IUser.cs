using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IUser
    {
        #region Public Properties

        String DisplayName { get; }

        String Id { get; }

        Byte[] ImageData { get; }

        String ImageUrl { get; }

        String UniqueName { get; }

        String Url { get; }

        #endregion
    }
}