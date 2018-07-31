using System.Net.Http;

namespace BuildsAppReborn.Infrastructure
{
    public static class HttpResponseMessageExtensions
    {
        public static void ThrowIfUnsuccessful(this HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    responseMessage.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    throw new AdvancedHttpRequestException(responseMessage.StatusCode, ex.Message);
                }
            }
        }
    }
}