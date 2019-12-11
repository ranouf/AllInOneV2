using AllInOne.Common.Exceptions;
using AllInOne.Common.Logging;
using AllInOne.Common.Session;
using AllInOne.Servers.API.Filters.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace AllInOne.Servers.API.Filters
{

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILoggerService<ApiExceptionFilter> _logger;
        private readonly IUserSession _session;

        public ApiExceptionFilter(
            IWebHostEnvironment environment,
            ILoggerService<ApiExceptionFilter> logger,
            IUserSession session)
        {
            _environment = environment;
            _logger = logger;
            _session = session;
        }

        public override void OnException(ExceptionContext context)
        {
            ApiErrorDto apiError;
            var properties = new Dictionary<string, string>{
                { "UserId",  _session.UserId?.ToString() }
            };

            if (context.Exception is LocalException)
            {
                _logger.LogInformation(context.Exception.Message, properties);

                var ex = context.Exception as LocalException;
                context.Exception = null;
                apiError = new ApiErrorDto(ex.Message);
                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                _logger.LogWarning(context.Exception.Message, properties);

                apiError = new ApiErrorDto("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
            }
            else
            {
                _logger.LogError(context.Exception.Message, context.Exception, properties);

                // Unhandled errors
                if (_environment.IsDevelopment())
                {
                    apiError = new ApiErrorDto(
                        context.Exception.GetBaseException().Message,
                        context.Exception.StackTrace
                    );
                }
                else
                {
                    apiError = new ApiErrorDto(
                        "An unhandled error occurred.",
                        null
                    );
                }

                context.HttpContext.Response.StatusCode = 500;
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
