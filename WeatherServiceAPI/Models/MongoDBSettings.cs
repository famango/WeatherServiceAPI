namespace WeatherServiceAPI.Models
{
    public class MongoDBSettings
    {

        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string WeatherForecastCollectionName { get; set; } = null!;
        public string GeocodingCollectionName { get; set; } = null!;

    }
}
