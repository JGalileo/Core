using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System;
using Core.Helpers;

namespace Core.Filters
{
    public class MonitorFilterAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // do something before the action executes
            context.HttpContext.Items.Add("WebKit", DateTime.Now);
            var request = context.HttpContext.Request;
            var connection = context.HttpContext.Connection;
            var log = $@"{request.Method} {request.Path.Value}{(request.QueryString.HasValue ? $"?{request.QueryString.Value}" : string.Empty)} {request.ContentType}\t\t${connection.RemoteIpAddress.ToIpv4()}:{connection.RemotePort}";
            // TODO log

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