using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Athena.Middleware
{
    /// <summary>
    /// Better logging for HTTP Requests with Serilog. Licensed as Apache-2.0.
    /// https://github.com/datalust/serilog-middleware-example/blob/master/src/Datalust.SerilogMiddlewareExample/Diagnostics/SerilogMiddleware.cs
    /// </summary>
    public class SerilogHttpMiddleware
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        private static readonly ILogger Log = Serilog.Log.ForContext<SerilogHttpMiddleware>();
        
        private readonly RequestDelegate _next;

        public SerilogHttpMiddleware(RequestDelegate next) =>
            _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task Invoke(HttpContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));

            var start = Stopwatch.GetTimestamp();
            try
            {
                await _next(ctx);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = ctx.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(ctx) : Log;
                log.Write(level, MessageTemplate, ctx.Request.Method, ctx.Request.Path, statusCode, elapsedMs);
            }
            // Never caught, because `LogException()` returns false.
            catch (Exception ex) when (LogException(ctx, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex)) { }
        }

        private static bool LogException(HttpContext httpContext, double elapsedMs, Exception ex)
        {
            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, elapsedMs);

            return false;
        }

        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }

        private static double GetElapsedMilliseconds(long start, long stop) =>
            (stop - start) * 1000 / (double)Stopwatch.Frequency;
    }
}