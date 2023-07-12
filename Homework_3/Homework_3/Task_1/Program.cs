using System.Text.RegularExpressions;

List<Person> users = new List<Person>
{
	new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37, Phone = "380969430654" },
	new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41, Phone = "380969232512" },
	new() { Id = Guid.NewGuid().ToString(), Name = "Cate", Age = 19, Phone = "380969222542" },
	new() { Id = Guid.NewGuid().ToString(), Name = "Liam", Age = 22 , Phone = "380992440578"},
	new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24, Phone = "380969123789" }
};


var builder = WebApplication.CreateBuilder();
var app = builder.Build();


app.UseStaticFiles();

app.Run(async (context) =>
{
	var response = context.Response;
	var request = context.Request;
	var path = request.Path;
	string expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

	if (path == "/api/users" && request.Method == "GET")
	{
		await GetAllPeople(response);
	}
	else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
	{
		string? id = path.Value?.Split("/")[3];
		await GetPerson(id, response);
	}
	else if (path == "/api/users" && request.Method == "POST")
	{
		await CreatePerson(response, request);
	}
	else if (path == "/api/users" && request.Method == "PUT")
	{
		await UpdatePerson(response, request);
	}
	else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE")
	{
		string? id = path.Value?.Split("/")[3];
		await DeletePerson(id, response);
	}
	else
	{
		response.ContentType = "text/html; charset=utf-8";
		await response.SendFileAsync("html/index.html");
	}
});

app.Run();

async Task GetAllPeople(HttpResponse response)
{
	await response.WriteAsJsonAsync(users);
}

async Task GetPerson(string? id, HttpResponse response)
{
	Person? user = users.FirstOrDefault((u) => u.Id == id);
	if (user != null)
		await response.WriteAsJsonAsync(user);
	else
	{
		response.StatusCode = 404;
		await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
	}
}

async Task DeletePerson(string? id, HttpResponse response)
{
	Person? user = users.FirstOrDefault((u) => u.Id == id);
	if (user != null)
	{
		users.Remove(user);
		await response.WriteAsJsonAsync(user);
	}
	else
	{
		response.StatusCode = 404;
		await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
	}
}

async Task CreatePerson(HttpResponse response, HttpRequest request)
{
	try
	{
		var user = await request.ReadFromJsonAsync<Person>();
		if (user != null)
		{
			user.Id = Guid.NewGuid().ToString();
			users.Add(user);
			await response.WriteAsJsonAsync(user);
		}
		else
		{
			throw new Exception("Некорректные данные");
		}
	}
	catch (Exception)
	{
		response.StatusCode = 400;
		await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
	}
}

async Task UpdatePerson(HttpResponse response, HttpRequest request)
{
	try
	{
		Person? userData = await request.ReadFromJsonAsync<Person>();
		if (userData != null)
		{
			var user = users.FirstOrDefault(u => u.Id == userData.Id);
			if (user != null)
			{
				user.Age = userData.Age;
				user.Name = userData.Name;
				user.Phone = userData.Phone;
				await response.WriteAsJsonAsync(user);
			}
			else
			{
				response.StatusCode = 404;
				await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
			}
		}
		else
		{
			throw new Exception("Некорректные данные");
		}
	}
	catch (Exception)
	{
		response.StatusCode = 400;
		await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
	}
}

public class Person
{
	public string Id { get; set; } = "";
	public string Name { get; set; } = "";
	public string Phone { get; set; } = "";
	public int Age { get; set; }
}