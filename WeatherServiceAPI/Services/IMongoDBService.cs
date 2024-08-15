// <copyright file="IMongoDBService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Services
{
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Interface for MongoDBService.
    /// </summary>
    public interface IMongoDBService
    {
        /// <summary>
        /// GetRecentWeatherForecastByLocationAsync.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        /// <param name="expiresInMinutes">ExpiresInMinutes is for how long the query will return a match for a given search.</param>
        /// <returns>Returns most recent <see cref="WeatherForecast"/>that is equal or less then expiresInMinutes - current time.</returns>
        Task<WeatherForecast?> GetRecentWeatherForecastByLocationAsync(float latitude, float longitude, int expiresInMinutes);

        /// <summary>
        /// Async method for creating a WeatherForecastCollection record.
        /// </summary>
        /// <param name="weatherForecast"><see cref="WeatherForecast"/> instance to persist.</param>
        /// <returns>No return.</returns>
        Task CreateWeatherForecastAsync(WeatherForecast weatherForecast);

        /// <summary>
        /// Async method for returning CityCollection record by name.
        /// </summary>
        /// <param name="name">Name of the city.</param>
        /// <returns>Return <see cref="City"/> instance or null.</returns>
        Task<City?> GetCityByNameAsync(string name);

        /// <summary>
        /// Async method for creating a CityCollection record.
        /// </summary>
        /// <param name="city">Name of the city.</param>
        /// <returns>No return.</returns>
        Task CreateCityAsync(City city);
    }
}
