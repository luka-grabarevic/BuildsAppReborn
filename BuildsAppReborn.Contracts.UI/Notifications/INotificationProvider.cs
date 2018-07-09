using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI.Notifications
{
    public interface INotificationProvider
    {
        Boolean IsSupported { get; }

        void ShowBuild(IBuild build, Func<IBuild, String> iconProvider, Action<IBuild> notificationClickAction);

        void ShowMessage(String title, String message);

        void ShowMessage(String title, String message, Action clickAction);
    }
}