using Athena.Exceptions;
using System.Net;

namespace Athena.Extensions
{
    public static class RepositoryExtensions
    {
        public static T NotFoundIfNull<T>(this T obj)
        {
            if (obj == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "The specified Object was not found");
            }

            return obj;
        }
    }
}
