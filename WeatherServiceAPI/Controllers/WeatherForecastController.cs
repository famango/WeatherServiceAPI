// <copyright file="WeatherForecastController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Controllers
{
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;
    using WeatherServiceAPI.Mappers;
    using WeatherServiceAPI.Services;

    /// <summary>
    /// Controller for Weather Forecast calls.
    /// </summary>
    [Route("api/weatherforecast")]
    public class WeatherForecastController : Controller
    {
        private readonly WeatherForecastService weatherForecastService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="weatherForcastService">Service class for Weather Forecast.</param>
        /// <param name="logger">Service claas for ILogger</param>
        public WeatherForecastController(WeatherForecastService weatherForcastService, ILogger logger)
        {
            this.weatherForecastService = weatherForcastService;
            this.logger = logger;
        }

        /// <summary>
        /// Get Weather Forecast by Location (latitue,longitude).
        /// </summary>
        /// <param name="latitude">Location latitude.</param>
        /// <param name="longitude">Location longitude.</param>
        /// <returns>BadRequest if invalid parameters, NotFound if the Location is not found,Ok with Weather Forcast if succesfull.</returns>
        [HttpGet]
        [Route("bylocation")]
        public async Task<IActionResult> GetByLocation([FromQuery] float latitude, float longitude)
        {
            this.logger.LogInformation("Processing request GetByLocation for Latitude:{Latitude}, and Longitude:{Longitude}.", latitude, longitude);
            if (latitude < -90 || latitude > 90)
            {
                this.ModelState.AddModelError("latitude", "Latitude is not a valid value.");
            }

            if (longitude < -180 || longitude > 180)
            {
                this.ModelState.AddModelError("longitude", "Longitude is not a valid value.");
            }

            if (!this.ModelState.IsValid)
            {
                this.logger.LogInformation("Completed request GetByLocation for Latitude:{Latitude}, and Longitude:{Longitude} with error.", latitude, longitude);
                return this.BadRequest(this.ModelState);
            }

            var result = await this.weatherForecastService.GetForecastByLocationAsync(latitude, longitude);

            if (result == null)
            {
                this.logger.LogInformation("Completed request GetByLocation for Latitude:{Latitude}, and Longitude:{Longitude}. Location not found!", latitude, longitude);
                return this.NotFound();
            }

            this.logger.LogInformation("Completed request GetByLocation for Latitude:{Latitude}, and Longitude:{Longitude}.", latitude, longitude);
            return this.Ok(result.ToDto());
        }

        /// <summary>
        /// Get Weather Forecast by City.
        /// </summary>
        /// <param name="city">Name of a City.</param>
        /// <returns>BadRequest if invalid parameters, NotFound if the City name is not found,Ok with Weather Forcast if succesfull.</returns>
        [HttpGet]
        [Route("bycity")]
        public async Task<IActionResult> GetByCity([FromQuery] string city)
        {
            this.logger.LogInformation("Processing request GetByCity for City:{City} with error.", city);
            bool isParsedSuccessfully = int.TryParse(city, out _);

            if (isParsedSuccessfully)
            {
                // Validating a zip code was not entered instead of a city name. Zip code
                // functionality is not programmed.
                this.ModelState.AddModelError("city", $"{city} is not a valid value for city.");
            }

            if (!this.ModelState.IsValid)
            {
                this.logger.LogInformation("Completed request GetByCity for City:{City} with error.", city);
                return this.BadRequest(this.ModelState);
            }

            var result = await this.weatherForecastService.GetForecastByCityAsync(city.Trim());
            if (result == null)
            {
                this.logger.LogInformation("Completed request GetByCity for City:{City}. City not found!", city);
                return this.NotFound();
            }

            this.logger.LogInformation("Completed request GetByCity for City:{City}.", city);
            return this.Ok(result.ToDto());
        }
    }
}
