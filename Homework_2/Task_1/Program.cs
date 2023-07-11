var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"Current day of the year: {DateTime.Now.DayOfYear}");

app.Run();
