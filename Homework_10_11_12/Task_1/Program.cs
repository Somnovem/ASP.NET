using Microsoft.EntityFrameworkCore;
using Task_1.Filters;
using Task_1.Models;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddAuthentication("LoremIpsumAuthScheme")
       .AddCookie("LoremIpsumAuthScheme", options => {
           options.Cookie.Name = "LoremIpsumCookie";
           options.ExpireTimeSpan = TimeSpan.FromDays(14);
       });


// Add services to the container.
builder.Services.AddControllersWithViews(options => options.Filters.Add<CustomExceptionFilterAttribute>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
