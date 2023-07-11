using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

var restaurants = new List<Restaurant>{
	new (){
		Name =  "Restaurant A",
		Address =  "123 Main Street",
		Phone =  "(123) 456-7890",
		Email = "info@restaurantA.com",
		About = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc interdum sem et tincidunt posuere.",
		Hours = "Monday - Friday: 9:00 AM - 10:00 PM"
    },
	new (){
		Name =  "Restaurant B",
		Address =   "456 Elm Street",
		Phone = "(456) 789-0123",
		Email = "info@restaurantB.com",
		About = "Sed sed felis non nunc tincidunt auctor at eget justo. Nullam aliquam pretium nisi ut rutrum.",
		Hours = "Monday - Saturday: 10:00 AM - 11:00 PM"
	},
	new (){
		Name = "Restaurant C",
		Address = "789 Oak Street",
		Phone = "(789) 012-3456",
		Email = "info@restaurantC.com",
		About = "In lobortis tellus ac tortor placerat, et faucibus nisl pulvinar. Fusce consectetur mi ligula, ut.",
		Hours = "Tuesday - Sunday: 11:00 AM - 9:00 PM"
	}
};

app.Run(async (context) =>
{
	var response = context.Response;
	var request = context.Request;
	var path = request.Path;
	if (path == "/api/users" && request.Method == "GET")
	{
		await response.WriteAsJsonAsync(restaurants);
	}
	else {
		context.Response.ContentType = "text/html; charset=utf-8";
		await context.Response.SendFileAsync("html/index.html");
	}

});

app.Run();

public class Restaurant
{
	public string Name { get; set; } = "";
	public string Address { get; set; } = "";
	public string Phone { get; set; } = "";
	public string Email { get; set; } = "";
	public string About { get; set; } = "";
	public string Hours { get; set; } = "";
}