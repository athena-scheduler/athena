using Athena.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Athena.Filters
{
    public class ApiModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path;

            // Only Validate automatically for API controllers
            if (path.HasValue && path.Value.ToLowerInvariant().Contains("api/v"))
            {
                if (!context.ModelState.IsValid)
                {
                    throw new BadModelException(context.ModelState);
                }
            }
        }
    }
}