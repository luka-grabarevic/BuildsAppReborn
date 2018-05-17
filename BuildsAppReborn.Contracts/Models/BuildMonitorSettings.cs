using System;
using System.Collections.Generic;
using BuildsAppReborn.Contracts.Extensions;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BuildsAppReborn.Contracts.Models
{
    [JsonDictionary]
    public class BuildMonitorSettings : Dictionary<String, Object>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public BuildMonitorSettings()
        {
            SelectedBuildDefinitions = new List<IBuildDefinition>();
        }

        public BuildMonitorSettings(String buildProviderId)
            : this()
        {
            BuildProviderId = buildProviderId;
            UniqueId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Don't know how to get JSON to deserialize properties AND dictionary at the same time
        /// </summary>
        [JsonIgnore]
        public String BuildProviderId
        {
            get { return GetDefaultValueIfNotExists<String>(nameof(BuildProviderId)); }
            private set { this[nameof(BuildProviderId)] = value; }
        }

        [JsonIgnore]
        public ICollection<IBuildDefinition> SelectedBuildDefinitions
        {
            get { return GetValue<ICollection<IBuildDefinition>>(nameof(SelectedBuildDefinitions)); }
            private set { this[nameof(SelectedBuildDefinitions)] = value; }
        }

        /// <summary>
        /// Don't know how to get JSON to deserialize properties AND dictionary at the same time
        /// </summary>
        [JsonIgnore]
        public String UniqueId
        {
            get { return GetDefaultValueIfNotExists<String>(nameof(UniqueId)); }
            private set { this[nameof(UniqueId)] = value; }
        }

        /// <summary>
        /// Gets the default value if not exists.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>the value</returns>
        public TValue GetDefaultValueIfNotExists<TValue>(String key)
        {
            if (ContainsKey(key))
            {
                return (TValue) this[key];
            }

            return default(TValue);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="KeyNotFoundException">when key not found</exception>
        /// <returns>the value</returns>
        public TValue GetValue<TValue>(String key)
        {
            this.ThrowIfKeyNotExists(key);
            return (TValue) this[key];
        }

        /// <summary>
        /// Gets the value strict.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="KeyNotFoundException">when key not found</exception>
        /// <exception cref="NullReferenceException">when found value is null or empty string</exception>
        /// <returns>the value</returns>
        public TValue GetValueStrict<TValue>(String key)
        {
            this.ThrowIfKeyNotExistsAndValueEmpty(key);
            return (TValue) this[key];
        }
    }
}