// <copyright file="CurrentUnits.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json.Serialization;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Model Class for CurrentUnits used in <see cref="Current"/> model.
    /// </summary>
    public class CurrentUnits
    {
        /// <summary>
        /// Gets or sets Temperature_2m.
        /// </summary>
        [BsonElement("temperature_2m")]
        public string? Temperature_2m { get; set; }

        /// <summary>
        /// Gets or sets Wind_Speed_10m.
        /// </summary>
        [BsonElement("wind_speed_10m")]
        public string? Wind_Speed_10m { get; set; }

        /// <summary>
        /// Gets or sets Wind_Direction_10m.
        /// </summary>
        [BsonElement("wind_direction_10m")]
        public string? Wind_Direction_10m { get; set; }
    }
}
