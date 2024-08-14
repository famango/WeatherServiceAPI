// <copyright file="WeatherForecastMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Mappers
{
    using WeatherServiceAPI.Dtos;
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Mapper utility for Weather Forecast Model to DTO.
    /// </summary>
    public static class WeatherForecastMapper
    {
        /// <summary>
        /// Mapps <see cref="WeatherForecast"/> to <see cref="WeatherForecastDto"/>.
        /// </summary>
        /// <param name="model">Existing <see cref="WeatherForecast"/> instance. </param>
        /// <returns>Return new instance of <see cref="WeatherForecastDto"/>.</returns>
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
