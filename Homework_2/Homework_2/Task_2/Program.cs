var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => (char)(new Random().Next('a','z'+1)));

app.Run();
