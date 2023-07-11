using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

const string countriesUrl = "https://countryinfoapi.com/api/countries";

async Task<string> Get5RandomCountriesJsonAsync()
{
	using (HttpClient client = new HttpClient())
	{
		HttpResponseMessage response = await client.GetAsync(countriesUrl);
		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}
	}
	return null;
}

app.Run(async (context) => {
	var response = context.Response;
	var request = context.Request;
	var path = request.Path;
	if (path == "/api/users" && request.Method == "GET")
	{
		var jsonString = await Get5RandomCountriesJsonAsync();
		if (jsonString == null) response.WriteAsync("Service is currently unavaliable. Try again later.");
		else await response.WriteAsync(jsonString);
	}
	else
	{
		context.Response.ContentType = "text/html; charset=utf-8";
		await context.Response.SendFileAsync("html/index.html");
	}
});

app.Run();