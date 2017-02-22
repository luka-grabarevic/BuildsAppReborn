using System;

namespace BuildsAppReborn.Contracts.Models
{
    public interface IProject
    {
        #region Public Properties

        String Description { get; }

        String Id { get; }

        String Name { get; }

        String Url { get; }

        #endregion
    }
}