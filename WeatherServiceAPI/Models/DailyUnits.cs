using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherServiceAPI.Models
{
    public class DailyUnits
    {
        [BsonElement("sunrise")]
        public string? Sunrise { get; set; }

        [BsonElement("temperature_2m_max")]
        [JsonPropertyName("temperature_2m_max")]
        public string? TemperatureMax { get; set; }

        [BsonElement("temperature_2m_min")]
        [JsonPropertyName("temperature_2m_min")]
        public string? TemperatureMin { get; set; }

        [BsonElement("wind_speed_10m_max")]
        [JsonPropertyName("wind_speed_10m_max")]
        public string? WindspeedMax { get; set; }

        [BsonElement("wind_direction_10m_dominant")]
        [JsonPropertyName("wind_direction_10m_dominant")]
        public string? WindDirection { get; set; }
    }
}
