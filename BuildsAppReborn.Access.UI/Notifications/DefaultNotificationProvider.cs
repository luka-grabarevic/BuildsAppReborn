using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows;

using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;

using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Messages.Core;
using ToastNotifications.Position;

namespace BuildsAppReborn.Access.UI.Notifications
{
    [PriorityExport(typeof(INotificationProvider), -1)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class DefaultNotificationProvider : INotificationProvider
    {
        #region Constructors

        public DefaultNotificationProvider()
        {
            this.notifier = new Notifier(cfg =>
                                             {
                                                 cfg.PositionProvider = new PrimaryScreenPositionProvider(Corner.BottomRight, 10, 50);
                                                 cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(10), MaximumNotificationCount.FromCount(3));
                                                 cfg.Dispatcher = Application.Current.Dispatcher;
                                             });
        }

        #endregion

        #region Implementation of INotificationProvider

        public void ShowBuild(IBuild build, Func<IBuild, String> iconProvider, Action<IBuild> notificationClickAction)
        {
            if (build == null) return;

            var sb = new StringBuilder();
            sb.AppendLine($"{build.Definition.Project.Name} - {build.Definition.Name}");
            sb.AppendLine(build.Status.ToString());
            sb.AppendLine(build.Requester.DisplayName);
            var displayOptions = new MessageOptions { NotificationClickAction = n =>
                                                                                    {
                                                                                        notificationClickAction?.Invoke(build);
                                                                                        n.Close();
                                                                                    } };

            switch (build.Status)
            {
                case BuildStatus.PartiallySucceeded:
                    this.notifier.ShowWarning(sb.ToString(), displayOptions);
                    break;
                case BuildStatus.Running:
                case BuildStatus.Stopped:
                case BuildStatus.Queued:
                case BuildStatus.Unknown:
                    this.notifier.ShowInformation(sb.ToString(), displayOptions);
                    break;
                case BuildStatus.Succeeded:
                    this.notifier.ShowSuccess(sb.ToString(), displayOptions);
                    break;
                case BuildStatus.Failed:
                    this.notifier.ShowError(sb.ToString(), displayOptions);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ShowMessage(String title, String message)
        {
            var sb = new StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine(message);
            this.notifier.ShowInformation(sb.ToString());
        }

        public Boolean IsSupported => true;

        #endregion

        #region Private Fields

        private Notifier notifier;

        #endregion
    }
}