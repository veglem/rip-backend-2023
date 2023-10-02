using System.Reflection;
using Microsoft.OpenApi.Models;
using RIP_lab01.Controllers;
using RIP_lab01.Database;
using RIP_lab01.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Version = "v1",
//         Title = "Shop API",
//         Description = "Пример ASP .NET Core Web API",
//         Contact = new OpenApiContact
//         {
//             Name = "Пример контакта",
//             Url = new Uri("https://example.com/contact")
//         },
//         License = new OpenApiLicense
//         {
//             Name = "Пример лицензии",
//             Url = new Uri("https://example.com/license")
//         }
//     });
//     var basePath = AppContext.BaseDirectory;
//
//     var xmlPath = Path.Combine(basePath, "RIP_lab01.xml");
//     options.IncludeXmlComments(xmlPath);
// });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger().UseSwaggerUI(options =>
    // {
    //     options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //     options.RoutePrefix = string.Empty;
    // });
}

app.UseStaticFiles();

// var x = await JsonDB.AsyncRead();
//
// System.IO.File.WriteAllText(@"db.json",string.Empty);
//
// JsonDB.AsyncWrite(x);

app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}");


// app.UseRouting();
// app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
