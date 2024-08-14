// <copyright file="WeatherForecastDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Dtos
{
    /// <summary>
    /// DTO for Weather Forecast.
    /// </summary>
    public class WeatherForecastDto
    {
        /// <summary>
        /// Gets or sets Temperature.
        /// </summary>
        public string Tempeture { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets WindDirection.
        /// </summary>
        public string WindDirection { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets WindSpeed.
        /// </summary>
        public string WindSpeed { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Sunrise.
        /// </summary>
        public string Sunrise { get; set; } = string.Empty;
    }
}
