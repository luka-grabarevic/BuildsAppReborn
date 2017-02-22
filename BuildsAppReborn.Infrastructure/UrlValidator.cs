using System;

namespace BuildsAppReborn.Infrastructure
{
    public static class UrlValidator
    {
        #region Public Static Methods

        // https://stackoverflow.com/a/7581824
        public static Boolean IsValidUrl(String urlString)
        {
            Uri uriResult;
            return Uri.TryCreate(urlString, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        #endregion
    }
}