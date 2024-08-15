// <copyright file="IWeatherForecast.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Services
{
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Interface IWeatherForecast.
    /// </summary>
    public interface IWeatherForecastService
    {
        /// <summary>
        /// Async method to get <see cref="WeatherForecast"/> by Location.
        /// </summary>
        /// <param name="latitude">Location latitude.</param>
        /// <param name="longitude">Location longitude.</param>
        /// <returns>Returns <see cref="WeatherForecast"/> or null.</returns>
        Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude);

        /// <summary>
        /// Async method to get <see cref="WeatherForecast"/> by Location.
        /// </summary>
        /// <param name="city">Name of the city.</param>
        /// <returns>Returns <see cref="WeatherForecast"/> or null.</returns>
        Task<WeatherForecast?> GetForecastByCityAsync(string city);
    }
}
