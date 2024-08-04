using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherServiceAPI.Models
{
    public class CurrentUnits
    {
        [BsonElement("temperature_2m")]
        public string? Temperature_2m { get; set; }

        [BsonElement("wind_speed_10m")]
        public string? Wind_Speed_10m { get; set; }

        [BsonElement("wind_direction_10m")]
        public string? Wind_Direction_10m { get; set; }
    }
}
