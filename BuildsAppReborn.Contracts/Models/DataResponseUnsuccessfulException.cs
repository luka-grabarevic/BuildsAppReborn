using System;
using System.Net;

namespace BuildsAppReborn.Contracts.Models
{
    public class DataResponseUnsuccessfulException : Exception
    {
        public DataResponseUnsuccessfulException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}