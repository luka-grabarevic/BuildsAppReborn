using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI.Notifications
{
    public interface INotificationProvider
    {
        #region Public Methods

        void ShowBuild(IBuild build, Func<IBuild, String> iconProvider);

        void ShowMessage(String title, String message);

        #endregion
    }
}