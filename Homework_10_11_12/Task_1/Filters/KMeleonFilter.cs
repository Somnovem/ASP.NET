using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Task_1.Filters
{
    public class KMeleonFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext _) { }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            if (Regex.IsMatch(userAgent, "K-Meleon"))
            {
                context.Result = new ContentResult { Content = "K-Meleon is forbidden." };
            }
        }
    }
}
