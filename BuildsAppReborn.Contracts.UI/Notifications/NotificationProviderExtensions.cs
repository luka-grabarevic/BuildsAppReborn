using System.Linq;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Contracts.UI.Notifications
{
    public static class NotificationProviderExtensions
    {
        public static INotificationProvider GetSupportedNotificationProvider(this LazyContainer<INotificationProvider, IPriorityMetadata> notificationProviders)
        {
            var orderedKeys = notificationProviders.Keys.OrderByDescending(a => a.Priority);
            foreach (var priorityMetadata in orderedKeys)
            {
                var provider = notificationProviders.GetSingleOrDefault(a => a == priorityMetadata);
                if (provider.IsSupported)
                {
                    return provider;
                }
            }

            return null;
        }
    }
}