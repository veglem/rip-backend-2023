using Newtonsoft.Json;
using WebApi;
using WebApi.AppServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHandlers();
builder.Services.AddRepositories();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = "swagger";
    config.SwaggerEndpoint("v1/swagger.json", "webApi");
});

app.UseRouting();

app.MapSwagger();

app.MapControllers();

app.Run();
