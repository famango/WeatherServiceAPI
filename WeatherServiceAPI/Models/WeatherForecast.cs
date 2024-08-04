using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherServiceAPI.Models
{
    public class WeatherForecast
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("latitude")]
        public float Latitude { get; set; }

        [BsonElement("longitude")]
        public float Longitude { get; set; }

        [BsonElement("searchedbylatitude")]
        public float? SearchedByLatitude { get; set; }

        [BsonElement("searchedbylongitude")]
        public float? SearchedByLongitude { get; set; }

        [BsonElement("searchedtimestamp")]
        public DateTime? SearchedTimeStamp { get; set; }

        [BsonElement("generationtime_ms")]
        [JsonPropertyName("generationtime_ms")]
        public float GenerationTimeMs { get; set; }

        [BsonElement("utc_offset_seconds")]
        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [BsonElement("timezone")]
        public string? Timezone { get; set; }

        [BsonElement("timezone_abbreviation")]
        [JsonPropertyName("timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }

        [BsonElement("current")]
        [JsonPropertyName("current")]
        public Current? Current { get; set; }

        [BsonElement("current_units")]
        [JsonPropertyName("current_units")]
        public CurrentUnits? CurrentUnits { get; set; }

        [BsonElement("daily")]
        [JsonPropertyName("daily")]
        public Daily? Daily { get; set; }

        [BsonElement("daily_units")]
        [JsonPropertyName("daily_units")]
        public DailyUnits? DailyUnits { get; set; }
    }
}
