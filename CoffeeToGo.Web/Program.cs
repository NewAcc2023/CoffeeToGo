using CoffeeToGo.EntityFramework;
using CoffeeToGo.Web.Repositories;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//SQL Connection
builder.Services.AddDbContext<CoffeeToGo.EntityFramework.DbContexts.AppDbContext>(c => {
	c.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeDb"));
	c.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<OrderRepo>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.Configure<RazorViewEngineOptions>(o =>
{
	o.ViewLocationFormats.Clear();
	o.ViewLocationFormats.Add("/UI/Home/{0}" + RazorViewEngine.ViewExtension);

	o.ViewLocationFormats.Add("/UI/Shared/{0}" + RazorViewEngine.ViewExtension);
});

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

app.UseAuthorization();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
