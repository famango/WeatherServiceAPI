// <copyright file="City.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json.Serialization;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Model class for City.
    /// </summary>
    public class City
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets CityId.
        /// </summary>
        [BsonElement("cityid")]
        [JsonPropertyName("id")]
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets SearchedByName.
        /// </summary>
        [BsonElement("searchedbyname")]
        public string SearchedByName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Timezone.
        /// </summary>
        [BsonElement("latitude")]
        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets Timezone.
        /// </summary>
        [BsonElement("longitude")]
        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets Timezone.
        /// </summary>
        [BsonElement("timezone")]
        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
    }
}
