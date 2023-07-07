using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Homework_1.Pages
{
    public class Task4Model : PageModel
    {
		private readonly IConfiguration Configuration;

		public Task4Model(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string Message1 { get; set; }
		public string Message2 { get; set; }
		public string Message3 { get; set; }
		public string Message4 { get; set; }
		public void OnGet()
        {
			var testKeyValue = Configuration["TestKey"];
			var age = Configuration["TestObj:Age"];
			var name = Configuration["TestObj:Name"];
			var aspNetCore = Configuration["Logging:LogLevel:Microsoft.AspNetCore"];

			Message1 = $"TestKey value: {testKeyValue}";
			Message2 = $"Name: {name}";
			Message3 = $"Age: {age}";
			Message4 = $"ASP.NET Core: {aspNetCore}";
		}
    }
}
