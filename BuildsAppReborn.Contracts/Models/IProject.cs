using System;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IProject : IObjectItem
    {
        #region Public Properties

        String Description { get; }

        String Id { get; }

        String Name { get; }

        #endregion
    }
}