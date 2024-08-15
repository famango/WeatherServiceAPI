namespace WeatherServiceAPI.Services
{
    using System.Text.Json;
    using System.Web;
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Interface for OpenMeteoClientService.
    /// </summary>
    public interface IOpenMeteoClientService
    {
        /// <summary>
        /// Async method to get <see cref="WeatherForecast"/>  by location.
        /// </summary>
        /// <param name="latitude">Location latitude.</param>
        /// <param name="longitude">Location longitude.</param>
        /// <returns>Returns <see cref="WeatherForecast"/> or null.</returns>
        Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude);

        /// <summary>
        /// Async method to get <see cref="City"/>  by city name.
        /// </summary>
        /// <param name="city">City name.</param>
        /// <returns><see cref="City"/>.</returns>
        Task<City?> GetCityByNameAsync(string city);
    }
}
