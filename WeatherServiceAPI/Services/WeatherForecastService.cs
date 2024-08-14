// <copyright file="WeatherForecastService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Services
{
    using Microsoft.Extensions.Options;
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Class WeatherForecastService.
    /// </summary>
    public class WeatherForecastService : IWeatherForecast
    {
        private readonly OpenMeteoClientService openMeteoClientService;
        private readonly MongoDBService mongoDBService;
        private readonly int weatherForecastDbExpiresMintues;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastService"/> class.
        /// </summary>
        /// <param name="openMeteoClientService">Service class OpenMeteoClientService.</param>
        /// <param name="mongoDBService">Service class MongoDBService.</param>
        /// <param name="weatherForecastSettgins">Service class WeatherForecastSettings of type IOptions.</param>
        public WeatherForecastService(OpenMeteoClientService openMeteoClientService, MongoDBService mongoDBService, IOptions<WeatherForecastSettings> weatherForecastSettgins)
        {
            this.openMeteoClientService = openMeteoClientService;
            this.mongoDBService = mongoDBService;
            this.weatherForecastDbExpiresMintues = weatherForecastSettgins.Value.WeatherForecastDbExpiresMintues;
        }

        /// <summary>
        /// Async method for getting <see cref="WeatherForecast"/> by city name.
        /// </summary>
        /// <param name="city">City name.</param>
        /// <returns>Returns <see cref="WeatherForecast"/> or null.</returns>
        public async Task<WeatherForecast?> GetForecastByCityAsync(string city)
        {
            City? cityModel;

            // Searching City name in Database.
            cityModel = await this.mongoDBService.GetCityByNameAsync(city);
            if (cityModel == null)
            {
                // City name not found in Database.
                // Fetching city data from OpenMeteo API.
                cityModel = await this.openMeteoClientService.GetCityByNameAsync(city);
                if (cityModel != null)
                {
                    // OpenMeteo return City data.
                    // Now persisting City in database.
                    await this.mongoDBService.CreateCityAsync(cityModel);
                }
            }

            if (cityModel != null)
            {
                return await this.GetForecastAsync(cityModel.Latitude, cityModel.Longitude);
            }

            return null;
        }

        /// <summary>
        /// Async method for getting <see cref="WeatherForecast"/>  by location.
        /// </summary>
        /// <param name="latitude">Location latitude.</param>
        /// <param name="longitude">Location longitude.</param>
        /// <returns>Returns <see cref="WeatherForecast"/>  or null.</returns>
        public async Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude)
        {
            return await this.GetForecastAsync(latitude, longitude);
        }

        private async Task<WeatherForecast?> GetForecastAsync(float latitude, float longitude)
        {
            // Searching location in database.
            var weatherForcast = await this.mongoDBService.GetRecentWeatherForecastByLocationAsync(latitude, longitude, this.weatherForecastDbExpiresMintues);
            if (weatherForcast == null)
            {
                // Location not found or stale data location.
                // Now fetching location from OpenMeteo API.
                weatherForcast = await this.openMeteoClientService.GetForecastByLocationAsync(latitude, longitude);
                if (weatherForcast != null)
                {
                    // OpenMeteo sucessfully return location
                    // Now persisting Weather Forecast in database.
                    await this.mongoDBService.CreateWeatherForecastAsync(weatherForcast);
                }
            }

            return weatherForcast;
        }
    }
}
