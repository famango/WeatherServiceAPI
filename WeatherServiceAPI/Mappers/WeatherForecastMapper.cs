using System.Runtime.CompilerServices;
using WeatherServiceAPI.Dtos;
using WeatherServiceAPI.Models;

namespace WeatherServiceAPI.Mappers
{
    public static class WeatherForecastMapper
    {
        public static WeatherForecastDto ToDto(this WeatherForecast model)
        {
            return new WeatherForecastDto
            {
                Tempeture = $"{model.Daily?.TemperatureMax?[0].ToString()} {model.DailyUnits?.TemperatureMax}.",
                WindDirection = $"{model.Daily?.WindDirection?[0].ToString()} {model.DailyUnits?.WindDirection}",
                WindSpeed = $"{model.Daily?.WindSpeed?[0].ToString()} {model.DailyUnits?.WindspeedMax}",
                Sunrise = $"{model.Daily?.Sunrise?[0].ToString()}",
            };
        }
    }
}
