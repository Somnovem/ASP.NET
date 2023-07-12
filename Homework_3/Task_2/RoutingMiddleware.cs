public class RoutingMiddleware
{
	readonly RequestDelegate next;
	public RoutingMiddleware(RequestDelegate next)
	{
		this.next = next;
	}
	public async Task InvokeAsync(HttpContext context)
	{
		var response = context.Response;
		var request = context.Request;
		string path = context.Request.Path;
		if (path.StartsWith("/index"))
		{
			response.ContentType = "text/html; charset=utf-8";

			if (path == "/index/upload" && request.Method == "POST" && request.Query["token"] == "555555")
			{
				IFormFileCollection files = request.Form.Files;
				var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
				Directory.CreateDirectory(uploadPath);
				foreach (var file in files)
				{
					string directoryPath = $"{uploadPath}/{file.FileName.Split('.').Last()}s";
					Directory.CreateDirectory(directoryPath);
					string fullPath = $"{directoryPath}/{file.FileName}";
					using (var fileStream = new FileStream(fullPath, FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}
				}
				response.Redirect($"/index?token={request.Query["token"]}");
			}
			await response.SendFileAsync("html/index.html");
		}
		else
		{
			response.StatusCode = 404;
		}
	}
}