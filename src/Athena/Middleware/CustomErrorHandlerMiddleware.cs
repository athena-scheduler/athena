using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            catch (BadModelException e)
            {
                ctx.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                ctx.Response.ContentType = "application/json";
                
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(
                    e.ModelState.Keys.SelectMany(
                        k => e.ModelState[k].Errors.Select(
                            er => new {Key = k, Error = er.ErrorMessage}
                        )
                    ).ToDictionary(k => k.Key, v => v.Error)
                ));
            }
            catch (ApiException e)
            {
                ctx.Response.StatusCode = (int) e.ResponseCode;
                ctx.Response.ContentType = "application/json";

                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(
                    new {message = e.Message}
                ));
            }
        }
    }
}