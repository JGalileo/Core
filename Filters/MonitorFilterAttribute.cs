using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Core.Filters
{
    public class MonitorFilterAttribute : IAsyncActionFilter
    {
        private readonly ILogger<MonitorFilterAttribute> _logger;
        public MonitorFilterAttribute(ILogger<MonitorFilterAttribute> logger) => _logger = logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // do something before the action executes
            context.HttpContext.Items.Add("WebKit", DateTime.Now);
            var request = context.HttpContext.Request;
            var connection = context.HttpContext.Connection;
            _logger.LogInformation(default(EventId), $"{request.Method} {request.Path.Value}{(request.QueryString.HasValue ? $"?{request.QueryString.Value}" : string.Empty)} {request.Protocol} {request.ContentType} {connection.RemoteIpAddress.ToIpv4()}:{connection.RemotePort}");

            var resultContext = await next();

            // do something after the action executes; resultContext.Result will be set
            if (resultContext.Result is ObjectResult result)
            {
                resultContext.Result = new ObjectResult(new
                {
                    value = result.Value,
                    span = DateTime.Now - (DateTime)context.HttpContext.Items["WebKit"],
                });
            }
        }
    }

}