using Microsoft.EntityFrameworkCore;
using Task_1.Filters;
using Task_1.Models;
using Task_1.Logging;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddAuthentication("LoremIpsumAuthScheme")
       .AddCookie("LoremIpsumAuthScheme", options => {
           options.Cookie.Name = "LoremIpsumCookie";
           options.ExpireTimeSpan = TimeSpan.FromDays(14);
       });

builder.Services.AddControllersWithViews(options => options.Filters.Add<CustomExceptionFilterAttribute>());

string logsPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

if (!Directory.Exists(logsPath))
{
    Directory.CreateDirectory(logsPath);
}

builder.Logging.AddFile(Path.Combine(logsPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();