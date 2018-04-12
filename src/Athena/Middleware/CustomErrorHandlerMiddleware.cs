using System.Net;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Athena.Middleware
{
    public class CustomErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomErrorHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (DuplicateObjectException e)
            {
                ctx.Response.StatusCode = (int) HttpStatusCode.Conflict;

                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(
                    new {message = e.Message}
                ));
            }
            catch (DependentObjectException e)
            {
                ctx.Response.StatusCode = (int) HttpStatusCode.PreconditionFailed;

                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(
                    new {message = e.Message}
                ));
            }
            catch (ApiException e)
            {
                ctx.Response.StatusCode = (int) e.ResponseCode;
                ctx.Response.ContentType = "application/json";

                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(
                    new
                    {
                        message = e.Message,
                        details = e.Payload
                    }
                ));
            }
        }
    }
}