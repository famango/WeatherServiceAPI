using WeatherServiceAPI.Models;

namespace WeatherServiceAPI.Services
{
    public interface IWeatherForecast
    {
        Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude);
        Task<WeatherForecast?> GetForecastByCityAsync(string city);
    }
}
