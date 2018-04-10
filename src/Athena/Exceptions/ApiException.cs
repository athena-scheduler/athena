using System;
using System.Net;

namespace Athena.Exceptions
{
    public class ApiException : Exception
    {
        public readonly HttpStatusCode ResponseCode;
        public readonly object Payload;

        public ApiException(HttpStatusCode status, string message, object payload = null) : base(message)
        {
            ResponseCode = status;
            Payload = payload;
        }
    }
}