using System.Net;
using Camera.Localize;
using Newtonsoft.Json.Linq;

using Joint.Data.Models;

namespace Camera
{
    public class MiddlewareError
    {
        private readonly RequestDelegate next;

        public MiddlewareError(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IHostEnvironment env /* other dependencies */)
        {
            try
            {
                
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/plain";// "text/plain";
            var error = !string.IsNullOrEmpty(exception.Message) ? exception.Message : exception.ToString();
            var msg = $"{Resources.error} : ${error}";  
            
            return context.Response.WriteAsync(msg);
        }
    }
}