// <copyright file="MongoDBService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Services
{
    using System.Formats.Asn1;
    using System.Runtime.InteropServices;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using WeatherServiceAPI.Controllers;
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Class MongoDBService for managing interaction with Database.
    /// </summary>
    public class MongoDBService
    {
        private readonly ILogger logger;
        private readonly IMongoCollection<WeatherForecast> weatherForecastCollection;
        private readonly IMongoCollection<City> citiesCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Service class for ILoggerFactory.</param>
        /// <param name="mongoDBSettings">Service class for IOptions of type MongoDBSettings.</param>
        public MongoDBService(ILoggerFactory loggerFactory, IOptions<MongoDBSettings> mongoDBSettings)
        {
            this.logger = loggerFactory.CreateLogger<MongoDBService>();
            MongoClient client = new (mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            this.weatherForecastCollection = database.GetCollection<WeatherForecast>(mongoDBSettings.Value.WeatherForecastCollectionName);
            this.citiesCollection = database.GetCollection<City>(mongoDBSettings.Value.GeocodingCollectionName);
        }

        /// <summary>
        /// GetRecentWeatherForecastByLocationAsync.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        /// <param name="expiresInMinutes">ExpiresInMinutes is for how long the query will return a match for a given search.</param>
        /// <returns>Returns most recent <see cref="WeatherForecast"/>that is equal or less then expiresInMinutes - current time.</returns>
        public async Task<WeatherForecast?> GetRecentWeatherForecastByLocationAsync(float latitude, float longitude, int expiresInMinutes)
        {
            float smallRange = 0.00001F;
            this.logger.LogInformation("Searching location {Latitude}, {Longitude} in DB (expiresInMinutes={ExpiresInMinutes}).", latitude, longitude, expiresInMinutes);
            FilterDefinition<WeatherForecast> filter = Builders<WeatherForecast>
                .Filter.Where(x => x.SearchedByLatitude > (latitude - smallRange) && x.SearchedByLongitude > (longitude - smallRange) &&
                x.SearchedTimeStamp >= DateTime.UtcNow.AddMinutes(-expiresInMinutes));
            var result = await this.weatherForecastCollection.Find(filter).FirstOrDefaultAsync();
            if (result == null)
            {
                this.logger.LogInformation("Searching location {Latitude}, {Longitude} in DB (expiresInMinutes={ExpiresInMinutes}). Location not found or stale data.", latitude, longitude, expiresInMinutes);
            }

            this.logger.LogInformation("Searching location {Latitude}, {Longitude} in DB (expiresInMinutes={ExpiresInMinutes}). Location found.", latitude, longitude, expiresInMinutes);
            return result;
        }

        /// <summary>
        /// Async method for creating a WeatherForecastCollection record.
        /// </summary>
        /// <param name="weatherForecast"><see cref="WeatherForecast"/> instance to persist.</param>
        /// <returns>No return.</returns>
        public async Task CreateWeatherForecastAsync(WeatherForecast weatherForecast)
        {
            this.logger.LogInformation("Inserting new WeatherForecast record: {WeatherForecast}", weatherForecast);
            await this.weatherForecastCollection.InsertOneAsync(weatherForecast);
        }

        /// <summary>
        /// Async method for returning CityCollection record by name.
        /// </summary>
        /// <param name="name">Name of the city.</param>
        /// <returns>Return <see cref="City"/> instance or null.</returns>
        public async Task<City?> GetCityByNameAsync(string name)
        {
            this.logger.LogInformation("Searching {Name} city in DB", name);
            FilterDefinition<City> filter = Builders<City>.Filter.Where(x => x.SearchedByName.Equals(name.ToUpper()));
            var result = await this.citiesCollection.Find(filter).FirstOrDefaultAsync();
            if (result == null)
            {
                this.logger.LogInformation("Searching {Name} city in DB. City not found!", name);
            }

            this.logger.LogInformation("Searching {Name} city in DB. City was found.", name);
            return result;
        }

        /// <summary>
        /// Async method for creating a CityCollection record.
        /// </summary>
        /// <param name="city">Name of the city.</param>
        /// <returns>No return.</returns>
        public async Task CreateCityAsync(City city)
        {
            this.logger.LogInformation("Inserting new WeatherForecast record: {City}", city);
            await this.citiesCollection.InsertOneAsync(city);
        }
    }
}