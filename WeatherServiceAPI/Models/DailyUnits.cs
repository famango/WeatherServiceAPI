// <copyright file="DailyUnits.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json.Serialization;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Model Class for DailyUnits used in <see cref="Daily"/> model.
    /// </summary>
    public class DailyUnits
    {
        /// <summary>
        /// Gets or sets Sunrise.
        /// </summary>
        [BsonElement("sunrise")]
        public string? Sunrise { get; set; }

        /// <summary>
        /// Gets or sets TemperatureMax.
        /// </summary>
        [BsonElement("temperature_2m_max")]
        [JsonPropertyName("temperature_2m_max")]
        public string? TemperatureMax { get; set; }

        /// <summary>
        /// Gets or sets TemperatureMin.
        /// </summary>
        [BsonElement("temperature_2m_min")]
        [JsonPropertyName("temperature_2m_min")]
        public string? TemperatureMin { get; set; }

        /// <summary>
        /// Gets or sets WindspeedMax.
        /// </summary>
        [BsonElement("wind_speed_10m_max")]
        [JsonPropertyName("wind_speed_10m_max")]
        public string? WindspeedMax { get; set; }

        /// <summary>
        /// Gets or sets WindDirection.
        /// </summary>
        [BsonElement("wind_direction_10m_dominant")]
        [JsonPropertyName("wind_direction_10m_dominant")]
        public string? WindDirection { get; set; }
    }
}
