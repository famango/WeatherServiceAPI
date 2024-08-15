namespace WeatherServiceAPI.Extensions
{
    using System.Net;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.Extensions.Logging;
    using WeatherServiceAPI.Controllers;
    using WeatherServiceAPI.Models;
    using WeatherServiceAPI.Services;

    /// <summary>
    /// Static class ExceptionMiddlewareExtensions.
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Method for configuring exception handler.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="loggerFactory">Service for logger.</param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger<WeatherForecastController>();
                        logger.LogError(contextFeature.Error, "Something went wrong: ");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error.",
                        }.ToString());
                    }
                });
            });
        }
    }
}
