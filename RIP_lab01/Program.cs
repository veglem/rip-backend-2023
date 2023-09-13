using RIP_lab01.Controllers;
using RIP_lab01.Database;
using RIP_lab01.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

// var x = await JsonDB.AsyncRead();
//
// System.IO.File.WriteAllText(@"db.json",string.Empty);
//
// JsonDB.AsyncWrite(x);

app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
