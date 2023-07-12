var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
if (app.Environment.IsProduction()) app.UseMiddleware<RoutingMiddleware>();
else if (app.Environment.IsDevelopment())
{
	app.Run(async (context) =>
	{
		var response = context.Response;
		var request = context.Request;
		response.ContentType = "text/html; charset=utf-8";
		await response.SendFileAsync("html/dev.html");
	});
}
app.Run();
