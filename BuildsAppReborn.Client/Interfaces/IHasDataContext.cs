using System;

namespace BuildsAppReborn.Client.Interfaces
{
    public interface IHasDataContext
    {
        #region Public Properties

        Object DataContext { get; set; }

        #endregion
    }
}