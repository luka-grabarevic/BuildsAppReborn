using System;
using System.Net;

namespace BuildsAppReborn.Contracts.Models
{
    public class DataResponseUnsuccessfulException : Exception
    {
        #region Constructors

        public DataResponseUnsuccessfulException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        #endregion

        #region Public Properties

        public HttpStatusCode StatusCode { get; }

        #endregion
    }
}