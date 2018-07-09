using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI.Notifications;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace BuildsAppReborn.Access.UI.Notifications
{
    [PriorityExport(typeof(INotificationProvider), -1)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class DefaultNotificationProvider : NotificationProviderBase, INotificationProvider
    {
        [ImportingConstructor]
        public DefaultNotificationProvider(GeneralSettings generalSettings) : base(generalSettings)
        {
            this.notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new PrimaryScreenPositionProvider(Corner.BottomRight, 10, 50);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(10), MaximumNotificationCount.FromCount(3));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public Boolean IsSupported => true;

        public void ShowBuild(IBuild build, Func<IBuild, String> iconProvider, Action<IBuild> notificationClickAction)
        {
            if (build == null)
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine(GetTitle(build));
            sb.AppendLine(build.GenerateStatus());
            sb.AppendLine(build.GenerateUsername());
            var displayOptions = new MessageOptions
            {
                NotificationClickAction = n =>
                {
                    notificationClickAction?.Invoke(build);
                    n.Close();
                }
            };

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
            ShowMessage(title, message, null);
        }

        public void ShowMessage(String title, String message, Action clickAction)
        {
            var sb = new StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine(message);

            if (clickAction == null)
            {
                this.notifier.ShowInformation(sb.ToString());
            }
            else
            {
                var displayOptions = new MessageOptions
                {
                    NotificationClickAction = n =>
                    {
                        clickAction.Invoke();
                        n.Close();
                    }
                };
                this.notifier.ShowInformation(sb.ToString(), displayOptions);
            }
        }

        private readonly Notifier notifier;
    }
}