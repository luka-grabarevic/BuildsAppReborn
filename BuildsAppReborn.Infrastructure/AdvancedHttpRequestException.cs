using System;
using System.Net;
using System.Net.Http;

namespace BuildsAppReborn.Infrastructure
{
    [Serializable]
    public class AdvancedHttpRequestException : HttpRequestException
    {
        public AdvancedHttpRequestException() : base((String) null, (Exception) null)
        {
        }

        public AdvancedHttpRequestException(HttpStatusCode statusCode) : this(statusCode, (String) null, (Exception) null)
        {
        }

        public AdvancedHttpRequestException(HttpStatusCode statusCode, String message) : this(statusCode, message, (Exception) null)
        {
        }

        public AdvancedHttpRequestException(HttpStatusCode statusCode, String message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}