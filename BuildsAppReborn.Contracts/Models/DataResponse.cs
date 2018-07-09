using System;
using System.Net;

namespace BuildsAppReborn.Contracts.Models
{
    public class DataResponse<T>
    {
        public T Data { get; set; }

        public Boolean IsSuccessStatusCode
        {
            get
            {
                if (StatusCode >= HttpStatusCode.OK)
                {
                    return StatusCode <= (HttpStatusCode) 299;
                }

                return false;
            }
        }

        public HttpStatusCode StatusCode { get; set; }

        public void ThrowIfUnsuccessful()
        {
            if (!IsSuccessStatusCode)
            {
                throw new DataResponseUnsuccessfulException(StatusCode);
            }
        }
    }
}