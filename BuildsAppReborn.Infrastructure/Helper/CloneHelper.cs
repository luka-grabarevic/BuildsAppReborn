using Newtonsoft.Json;

namespace BuildsAppReborn.Infrastructure
{
    /// <summary>
    /// clone helper for tests
    /// </summary>
    public static class CloneHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>the cloned object</returns>
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source, Consts.JsonSerializerSettings);
            return JsonConvert.DeserializeObject<T>(serialized, Consts.JsonSerializerSettings);
        }

        #endregion
    }
}