using Microsoft.AspNetCore.Mvc.Filters;

namespace Task_1.Filters
{
    public class RequestTimeFilterAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext _) { }
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("RequestTime", DateTime.Now.ToString());
        }
    }

}
