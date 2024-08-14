// <copyright file="WeatherForecast.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json.Serialization;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Class WeatherForcast model.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets Latitude.
        /// </summary>
        [BsonElement("latitude")]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets Longitude.
        /// </summary>
        [BsonElement("longitude")]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets SearchedByLatitude.
        /// </summary>
        [BsonElement("searchedbylatitude")]
        public float? SearchedByLatitude { get; set; }

        /// <summary>
        /// Gets or sets SearchedByLongitude.
        /// </summary>
        [BsonElement("searchedbylongitude")]
        public float? SearchedByLongitude { get; set; }

        /// <summary>
        /// Gets or sets SearchedTimeStamp.
        /// </summary>
        [BsonElement("searchedtimestamp")]
        public DateTime? SearchedTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets GenerationTimeMs.
        /// </summary>
        [BsonElement("generationtime_ms")]
        [JsonPropertyName("generationtime_ms")]
        public float GenerationTimeMs { get; set; }

        /// <summary>
        /// Gets or sets UtcOffsetSeconds.
        /// </summary>
        [BsonElement("utc_offset_seconds")]
        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        /// <summary>
        /// Gets or sets Timezone.
        /// </summary>
        [BsonElement("timezone")]
        public string? Timezone { get; set; }

        /// <summary>
        /// Gets or sets TimezoneAbbreviation.
        /// </summary>
        [BsonElement("timezone_abbreviation")]
        [JsonPropertyName("timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets Current model.
        /// </summary>
        [BsonElement("current")]
        [JsonPropertyName("current")]
        public Current? Current { get; set; }

        /// <summary>
        /// Gets or sets CurrentUnits model.
        /// </summary>
        [BsonElement("current_units")]
        [JsonPropertyName("current_units")]
        public CurrentUnits? CurrentUnits { get; set; }

        /// <summary>
        /// Gets or sets Daily model.
        /// </summary>
        [BsonElement("daily")]
        [JsonPropertyName("daily")]
        public Daily? Daily { get; set; }

        /// <summary>
        /// Gets or sets DailyUnits model.
        /// </summary>
        [BsonElement("daily_units")]
        [JsonPropertyName("daily_units")]
        public DailyUnits? DailyUnits { get; set; }
    }
}
