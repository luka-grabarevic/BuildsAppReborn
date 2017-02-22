using System;
using System.Collections.Generic;

using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Extensions
{
    public static class BuildProviderSettingsExtensions
    {
        #region Public Static Methods

        public static void ThrowIfKeyNotExists(this BuildMonitorSettings settings, String key)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!settings.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Required key {key} not found in settings!");
            }
        }

        public static void ThrowIfKeyNotExistsAndValueEmpty(this BuildMonitorSettings settings, String key)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            settings.ThrowIfKeyNotExists(key);

            var value = settings[key];

            if (value == null || (value is String && String.IsNullOrEmpty(value.ToString())))
            {
                throw new NullReferenceException($"Value for key {key} must not be empty or null!");
            }
        }

        #endregion
    }
}