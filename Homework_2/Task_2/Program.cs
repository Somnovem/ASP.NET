var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

char getRandomLetter()
{
	char randomLetter = (char)(new Random().Next('a', 'z' + 1));

	if (new Random().Next(0, 2) == 0) randomLetter = (char)(randomLetter - 32);

	return randomLetter;
}

app.MapGet("/", () => $"Random letter: {getRandomLetter()}");

app.Run();
