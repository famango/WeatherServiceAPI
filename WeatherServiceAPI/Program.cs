using Microsoft.OpenApi.Models;
using Serilog;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<OpenMeteoClientSettings>(builder.Configuration.GetSection("OpenMeteoClient"));
builder.Services.Configure<WeatherForecastSettings>(builder.Configuration.GetSection("WeatherForecast"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather Service API", Version = "v1" });
});
builder.Services.AddScoped<WeatherForecastService>();
builder.Services.AddScoped<OpenMeteoClientService>();
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Weather Service API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
