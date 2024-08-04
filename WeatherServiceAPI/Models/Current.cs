using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WeatherServiceAPI.Models
{
    public class Current
    {
        [BsonElement("temperature_2m")]
        public float? Temperature_2m { get; set; }

        [BsonElement("wind_speed_10m")]
        public float? Wind_Speed_10m { get; set; }

        [BsonElement("wind_direction_10m")]
        public int? Wind_Direction_10m { get; set; }
    }
}
