using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WalletWatch.WebAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //logging reqest
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} from {context.Connection.RemoteIpAddress}");

            await _next(context);

            //logging response
            _logger.LogInformation($"Response: {context.Response.StatusCode}");
        }
    }
}
