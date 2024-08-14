// <copyright file="MongoDBSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    /// <summary>
    /// Class MongoDBSettings for managing <see cref="MongoDBService"/> settings.
    /// </summary>
    public class MongoDBSettings
    {
        /// <summary>
        /// Gets or sets ConnectionURI.
        /// </summary>
        public string ConnectionURI { get; set; } = null!;

        /// <summary>
        /// Gets or sets DatabaseName.
        /// </summary>
        public string DatabaseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets WeatherForecastCollectionName.
        /// </summary>
        public string WeatherForecastCollectionName { get; set; } = null!;

        /// <summary>
        /// Gets or sets GeocodingCollectionName.
        /// </summary>
        public string GeocodingCollectionName { get; set; } = null!;
    }
}
