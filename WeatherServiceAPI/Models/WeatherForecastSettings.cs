// <copyright file="WeatherForecastSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    /// <summary>
    /// Class WeatherForecastSettings for managing <see cref="WeatherForecastService"/> settings.
    /// </summary>
    public class WeatherForecastSettings
    {
        /// <summary>
        /// Gets or sets WeatherForecastDbExpiresMintues.
        /// </summary>
        public int WeatherForecastDbExpiresMintues { get; set; } = 0;
    }
}
