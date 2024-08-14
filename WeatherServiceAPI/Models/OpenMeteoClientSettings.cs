// <copyright file="OpenMeteoClientSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    /// <summary>
    /// Class OpenMeteoClientSettings for managing <see cref="OpenMeteoClientService"/> settings.
    /// </summary>
    public class OpenMeteoClientSettings
    {
        /// <summary>
        /// Gets or sets WeatherApiUrl.
        /// </summary>
        public string WeatherApiUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets GeocodingApiUrl.
        /// </summary>
        public string GeocodingApiUrl { get; set; } = null!;
    }
}
