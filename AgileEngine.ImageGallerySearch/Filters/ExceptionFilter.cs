using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult($"Han error happens. Please contact admin with the following id: '{MappedDiagnosticsLogicalContext.Get("RequestId")}'")
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = false;
            base.OnException(context);

            var logger = (ILogger<LogFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogFilter>));
            logger.LogError(context.Exception, context.Exception.Message);
        }
    }
}
