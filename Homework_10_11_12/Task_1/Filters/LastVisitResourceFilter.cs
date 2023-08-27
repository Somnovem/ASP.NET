using Microsoft.AspNetCore.Mvc.Filters;

namespace Task_1.Filters
{
    public class LastVisitResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.Now.ToString("dd/MM/yyyy_HH-mm-ss"));
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // реализация отсутствует
        }
    }
}
