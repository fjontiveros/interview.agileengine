using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Filters
{
    public class LogFilter : ActionFilterAttribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            var logger = (ILogger<LogFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogFilter>));
            logger.LogDebug($"Request Ends");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            MappedDiagnosticsLogicalContext.Set("RequestId", Guid.NewGuid());

            var logger = (ILogger<LogFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogFilter>));
            logger.LogDebug($"Request Begins");
        }
    }
}
