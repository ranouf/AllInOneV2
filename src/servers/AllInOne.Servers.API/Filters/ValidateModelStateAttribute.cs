using AllInOne.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace AllInOne.Servers.API.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new LocalException(context.ModelState.First().Value.Errors.First().ErrorMessage);
            }
        }
    }
}
