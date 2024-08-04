namespace WeatherServiceAPI.Models
{
    public class OpenMeteoClientSettings
    {
        public string WeatherApiUrl { get; set; } = null!;
        public string GeocodingApiUrl { get; set; } = null!;
    }
}
