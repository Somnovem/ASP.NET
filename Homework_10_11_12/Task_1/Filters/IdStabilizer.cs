using Microsoft.AspNetCore.Mvc.Filters;

namespace Task_1.Filters
{
    public class IdStabilizer : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if ((int)context.ActionArguments["id"] < 1)
            {
                context.ActionArguments["id"] = 1;
            }
            await next();
        }
    }
}
