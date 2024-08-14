// <copyright file="Geocoding.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Model class for Geocoding.
    /// </summary>
    public class Geocoding
    {
        /// <summary>
        /// Gets or sets Cities.
        /// </summary>
        [JsonPropertyName("results")]
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public City[]? Cities { get; set; }
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
    }
}
