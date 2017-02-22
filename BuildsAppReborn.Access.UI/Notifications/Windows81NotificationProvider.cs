using System;
using System.ComponentModel.Composition;
using System.IO;

using Windows.UI.Notifications;

using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;

namespace BuildsAppReborn.Access.UI.Notifications
{
    [Export(typeof(INotificationProvider))]
    internal class Windows81NotificationProvider : INotificationProvider
    {
        #region Implementation of INotificationProvider

        public void ShowBuild(IBuild build, Func<IBuild, String> iconProvider)
        {
            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");

            stringElements[0].AppendChild(toastXml.CreateTextNode($"{build.Definition.Project.Name} - {build.Definition.Name}"));
            stringElements[1].AppendChild(toastXml.CreateTextNode(build.Status.ToString()));
            stringElements[2].AppendChild(toastXml.CreateTextNode(build.Requester.DisplayName));

            // Specify the absolute path to an image
            var imagePath = "file:///" + Path.GetFullPath(iconProvider.Invoke(build));

            var imageElements = toastXml.GetElementsByTagName("image");
            var src = imageElements[0].Attributes.GetNamedItem("src");
            if (src != null) src.NodeValue = imagePath;

            // Create the toast and attach event listeners
            var toast = new ToastNotification(toastXml);
            toast.Dismissed += ToastDismissed;

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
        }

        #endregion

        #region Private Methods

        private void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs e)
        {
            // ToDo: open build etc.
            //switch (e.Reason)
            //{
            //    case ToastDismissalReason.ApplicationHidden:
            //        break;
            //    case ToastDismissalReason.UserCanceled:
            //        break;
            //    case ToastDismissalReason.TimedOut:
            //        break;
            //}
        }

        #endregion

        // Code partially from: https://www.whitebyte.info/programming/c/how-to-make-a-notification-from-a-c-desktop-application-in-windows-10

        private const String AppId = "BuildsAppReborn";
    }
}