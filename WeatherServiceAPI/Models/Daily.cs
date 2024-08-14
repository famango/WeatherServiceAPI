// <copyright file="Daily.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json.Serialization;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Model clas for Daily Weather Forecast.
    /// </summary>
    public class Daily
    {
        /// <summary>
        /// Gets or sets TemperatureMax.
        /// </summary>
        [BsonElement("temperature_2m_max")]
        [JsonPropertyName("temperature_2m_max")]
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public float[]? TemperatureMax { get; set; }

        /// <summary>
        /// Gets or sets TemperatureMin.
        /// </summary>
        [BsonElement("temperature_2m_min")]
        [JsonPropertyName("temperature_2m_min")]
        public float[]? TemperatureMin { get; set; }

        /// <summary>
        /// Gets or sets Sunrise.
        /// </summary>
        [BsonElement("sunrise")]
        public string[]? Sunrise { get; set; }

        /// <summary>
        /// Gets or sets WindSpeed.
        /// </summary>
        [BsonElement("wind_speed_10m_max")]
        [JsonPropertyName("wind_speed_10m_max")]
        public float[]? WindSpeed { get; set; }

        /// <summary>
        /// Gets or sets WindDirection.
        /// </summary>
        [BsonElement("wind_direction_10m_dominant")]
        [JsonPropertyName("wind_direction_10m_dominant")]
        public float[]? WindDirection { get; set; }
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
    }
}
