using System.Net;
using Microsoft.AspNetCore.Http;

namespace Athena.Extensions
{
    public static class RequestExtensions
    {
        private static bool IsSet(this IPAddress addr) => addr?.ToString() != "::1";
        
        public static bool IsLocal(this HttpRequest request)
        {
            var connection = request.HttpContext.Connection;

            return connection.RemoteIpAddress.IsSet()
                ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                : IPAddress.IsLoopback(connection.RemoteIpAddress);
        }
    }
}