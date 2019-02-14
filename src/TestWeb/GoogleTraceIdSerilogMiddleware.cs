using Google.Cloud.Diagnostics.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace TestWeb
{
    public class GoogleTraceIdSerilogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IManagedTracer _managedTracer;

        public GoogleTraceIdSerilogMiddleware(RequestDelegate next, IManagedTracer managedTracer)
        {
            _next = next;
            _managedTracer = managedTracer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (LogContext.PushProperty("googleTrace", _managedTracer.GetCurrentTraceId()))
            {
                await _next(context);
            }
        }
    }

    public static class GoogleTraceSerilogExtensions
    {
        public static IApplicationBuilder UseGoogleTraceIdInSerilog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GoogleTraceIdSerilogMiddleware>();
        }
    }
}
