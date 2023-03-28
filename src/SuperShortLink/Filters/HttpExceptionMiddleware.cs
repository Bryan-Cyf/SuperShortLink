using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SuperShortLink
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private IWebHostEnvironment environment;

        public HttpExceptionMiddleware(RequestDelegate next, ILogger<HttpExceptionMiddleware> logger, IWebHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
                var features = context.Features;
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";

            var error = ReadException(ex);
            logger.LogError(error);

            if (environment.IsDevelopment())
            {
                var json = new { message = ex.Message, detail = error };
                error = System.Text.Json.JsonSerializer.Serialize(json);
            }
            else
            {
                error = "系统繁忙，请稍后再试";
            }

            await context.Response.WriteAsync(error);
        }


        private string ReadException(Exception ex)
        {
            var error = string.Format("{0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException);
            if (ex.InnerException != null)
            {
                error += ReadException(ex.InnerException);
            }
            return error;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}
