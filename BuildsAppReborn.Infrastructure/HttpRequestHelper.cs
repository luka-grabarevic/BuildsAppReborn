using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuildsAppReborn.Infrastructure
{
    public static class HttpRequestHelper
    {
        #region Public Static Methods

        public static async Task<String> GetRequestResult(String url, ICredentials credentials)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = credentials;
                using (var client = new HttpClient(handler))
                {
                    var result = await client.GetAsync(url);
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStringAsync();
                    }
                    throw new Exception($"Request for {url} failed! StatusCode: {result.StatusCode}");
                }
            }
        }

        public static async Task<Byte[]> GetRequestResultsAsByteArray(String url, ICredentials credentials)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = credentials;
                using (var client = new HttpClient(handler))
                {
                    var result = await client.GetAsync(url);
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
                    }
                    throw new Exception($"Request for {url} failed! StatusCode: {result.StatusCode}");
                }
            }
        }

        #endregion
    }
}