using WeatherServiceAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Formats.Asn1;
using WeatherServiceAPI.Controllers;

namespace WeatherServiceAPI.Services
{
    public class MongoDBService
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMongoCollection<WeatherForecast> _weatherForecastCollection;
        private readonly IMongoCollection<City> _citiesCollection;

        public MongoDBService(ILogger<WeatherForecastController> logger, IOptions<MongoDBSettings> mongoDBSettings)
        {
            _logger = logger;
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _weatherForecastCollection = database.GetCollection<WeatherForecast>(mongoDBSettings.Value.WeatherForecastCollectionName);
            _citiesCollection = database.GetCollection<City>(mongoDBSettings.Value.GeocodingCollectionName);

        }

        /// <summary>
        /// GetRecentWeatherForecastByLocationAsync.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        /// <param name="expiresInMinutes">ExpiresInMinutes is for how long the query will return a match for a given search.</param>
        /// <returns>Returns most recent WeatherForecast that is equal or less then expiresInMinutes - current time.</returns>
        public async Task<WeatherForecast> GetRecentWeatherForecastByLocationAsync(float latitude, float longitude, int expiresInMinutes) {
            _logger.LogInformation($"Fetching coordinates {latitude.ToString()}, {longitude.ToString()} from DB");
            FilterDefinition<WeatherForecast> filter = Builders<WeatherForecast>.Filter.Where(
                x => x.SearchedByLatitude==latitude && 
                x.SearchedByLongitude==longitude && 
                x.SearchedTimeStamp >= DateTime.UtcNow.AddMinutes(-expiresInMinutes)
                );
            var result = await _weatherForecastCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }
        public async Task CreateWeatherForecastAsync(WeatherForecast weatherForecast) {
            await _weatherForecastCollection.InsertOneAsync(weatherForecast);
            return;
        }

        public async Task<City> GetCityByNameAsync(string name)
        {
            _logger.LogInformation($"Fetching {name} city from DB");
            FilterDefinition<City> filter = Builders<City>.Filter.Where(x => x.SearchedByName.Equals(name.ToUpper()));
            var result = await _citiesCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task CreateCityAsync(City city)
        {
            await _citiesCollection.InsertOneAsync(city);
            return;
        }

    }
}