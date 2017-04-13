using System;

using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI.Notifications
{
    public interface INotificationProvider
    {
        #region Public Properties

        Boolean IsSupported { get; }

        #endregion

        #region Public Methods

        void ShowBuild(IBuild build, Func<IBuild, String> iconProvider, Action<IBuild> notificationClickAction);

        void ShowMessage(String title, String message);

        #endregion
    }
}