using System;
using System.ComponentModel.Composition;
using System.IO;
using Windows.UI.Notifications;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;

namespace BuildsAppReborn.Access.UI.Notifications
{
    [PriorityExport(typeof(INotificationProvider), 100)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class Windows10NotificationProvider : NotificationProviderBase, INotificationProvider
    {
        #region Constructors

        [ImportingConstructor]
        public Windows10NotificationProvider(GeneralSettings generalSettings) : base(generalSettings)
        {
        }

        #endregion

        #region Implementation of INotificationProvider

        public void ShowBuild(IBuild build, Func<IBuild, String> iconProvider, Action<IBuild> notificationClickAction)
        {
            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");

            stringElements[0].AppendChild(toastXml.CreateTextNode(GetTitle(build)));
            stringElements[1].AppendChild(toastXml.CreateTextNode(build.GenerateStatus()));
            stringElements[2].AppendChild(toastXml.CreateTextNode(build.GenerateUsername()));

            // Specify the absolute path to an image
            var imagePath = "file:///" + Path.GetFullPath(iconProvider.Invoke(build));

            var imageElements = toastXml.GetElementsByTagName("image");
            var src = imageElements[0].Attributes.GetNamedItem("src");
            if (src != null) src.NodeValue = imagePath;

            // Create the toast and attach event listeners
            var toast = new ToastNotification(toastXml);
            if (notificationClickAction != null) toast.Activated += (sender, args) => { notificationClickAction.Invoke(build); };

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
        }

        public void ShowMessage(String title, String message)
        {
            ShowMessage(title, message, null);
        }

        public void ShowMessage(String title, String message, Action clickAction)
        {
            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");

            stringElements[0].AppendChild(toastXml.CreateTextNode(title));
            stringElements[1].AppendChild(toastXml.CreateTextNode(message));
            //stringElements[2].AppendChild(toastXml.CreateTextNode();

            // Create the toast and attach event listeners
            var toast = new ToastNotification(toastXml);
            if (clickAction != null) toast.Activated += (sender, args) => { clickAction.Invoke(); };

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
        }

        public Boolean IsSupported
        {
            get
            {
                // somehow windows version is not correctly provided when in debug, but release works
#if !DEBUG
                var os = Environment.OSVersion;
                return os.Version.Major >= 10;
#endif
#if DEBUG
                return true;
#endif
            }
        }

        #endregion

        // Code partially from: https://www.whitebyte.info/programming/c/how-to-make-a-notification-from-a-c-desktop-application-in-windows-10

        private const String AppId = "com.squirrel.BuildsAppReborn.BuildsAppReborn.Client";
    }
}