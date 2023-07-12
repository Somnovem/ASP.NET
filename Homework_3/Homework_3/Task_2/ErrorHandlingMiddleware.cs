public class ErrorHandlingMiddleware
{
	readonly RequestDelegate next;
	public ErrorHandlingMiddleware(RequestDelegate next)
	{
		this.next = next;
	}
	public async Task InvokeAsync(HttpContext context)
	{
		await next.Invoke(context);
		if (context.Response.StatusCode == 403)
		{
			await context.Response.WriteAsync("Access Denied");
		}
		else if (context.Response.StatusCode == 404)
		{
			context.Response.ContentType = "text/html; charset=utf-8";
			await context.Response.SendFileAsync("html/notfound.html");
		}
	}
}