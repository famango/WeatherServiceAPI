// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.OpenApi.Models;
using Serilog;
using WeatherServiceAPI.Extensions;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Services;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<OpenMeteoClientSettings>(builder.Configuration.GetSection("OpenMeteoClient"));
builder.Services.Configure<WeatherForecastSettings>(builder.Configuration.GetSection("WeatherForecast"));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(loggerConfig);
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather Service API", Version = "v1" });
});
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IOpenMeteoClientService, OpenMeteoClientService>();
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();
builder.Services.AddHttpClient();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

var logger = app.Services.GetRequiredService<ILoggerFactory>();
app.ConfigureExceptionHandler(logger);

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

#pragma warning disable S6966 // Awaitable method should be used
app.Run();
#pragma warning restore S6966 // Awaitable method should be used
