using System;
using System.Net;

namespace Athena.Exceptions
{
    public class ApiException : Exception
    {
        public readonly HttpStatusCode ResponseCode;
        private HttpStatusCode notFound;

        public ApiException(HttpStatusCode notFound)
        {
            this.notFound = notFound;
        }

        public ApiException(HttpStatusCode status, string message) : base(message) => ResponseCode = status;
    }
}