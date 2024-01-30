using Newtonsoft.Json;
using WebApi;
using WebApi.AppServices;

var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:5173");
        });
});

builder.Services.AddHandlers();
builder.Services.AddRepositories();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddS3(builder.Configuration);
builder.Services.AddAuth();
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error);
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "WebApi.Host.xml");
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = "swagger";
    config.SwaggerEndpoint("v1/swagger.json", "webApi");
});

app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization(); 

app.MapSwagger();

app.MapControllers();

app.Run();
