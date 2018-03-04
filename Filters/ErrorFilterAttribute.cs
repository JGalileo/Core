using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Filters
{
    public class ErrorFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public ErrorFilterAttribute(IHostingEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            var body = BodyString(context.HttpContext.Request.Body);
            // TODO log

            context.Result = new ObjectResult(new
            {
                value = new { Exception = context.Exception.Message, InnerException = context.Exception.InnerException?.Message, },
                span = DateTime.Now - (DateTime)context.HttpContext.Items["WebKit"],
            })
            {
                StatusCode = 200,
            };

            string BodyString(Stream stream)
            {
                if (stream == null)
                {
                    return null;
                }

                using (var ms = new MemoryStream())
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = 0;
                        var pos = (int)ms.Position;
                        var length = (int)(stream.Length - stream.Position) + pos;
                        ms.SetLength(length);
                        while (pos < length)
                        {
                            pos += stream.Read(ms.GetBuffer(), pos, length - pos);
                        }
                    }
                    else
                    {
                        stream.CopyTo(ms);
                    }
                    using (var streamReader = new StreamReader(ms))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

}