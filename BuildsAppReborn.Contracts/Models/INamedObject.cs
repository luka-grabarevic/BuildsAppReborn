using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface INamedObject
    {
        #region Public Properties

        String Name { get; }

        #endregion
    }
}