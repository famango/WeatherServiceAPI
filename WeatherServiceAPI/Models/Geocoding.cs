using System.Text.Json.Serialization;
namespace WeatherServiceAPI.Models
{
    public class Geocoding
    {
        [JsonPropertyName("results")]
        public City[]? Cities { get; set; }
    }
}
