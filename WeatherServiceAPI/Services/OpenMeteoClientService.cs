// <copyright file="OpenMeteoClientService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Services
{
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Web;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using WeatherServiceAPI.Controllers;
    using WeatherServiceAPI.Models;

    /// <summary>
    /// Class OpenMeteoClientService.
    /// </summary>
    public class OpenMeteoClientService : IOpenMeteoClientService
    {
        private readonly ILogger logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string weatherApiUrl;
        private readonly string geocodingApiUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenMeteoClientService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Service class for ILoggerFactory.</param>
        /// <param name="httpClientFactory">Service class for IHttpClientFactory.</param>
        /// <param name="openMeteoClientSettings">Service class for IOptions of type OpenMeteoClientSettings.</param>
        public OpenMeteoClientService(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory, IOptions<OpenMeteoClientSettings> openMeteoClientSettings)
        {
            this.logger = loggerFactory.CreateLogger<OpenMeteoClientService>();
            this.httpClientFactory = httpClientFactory;
            this.weatherApiUrl = openMeteoClientSettings.Value.WeatherApiUrl;
            this.geocodingApiUrl = openMeteoClientSettings.Value.GeocodingApiUrl;
        }

        /// <summary>
        /// Async method to get <see cref="WeatherForecast"/>  by location.
        /// </summary>
        /// <param name="latitude">Location latitude.</param>
        /// <param name="longitude">Location longitude.</param>
        /// <returns>Returns <see cref="WeatherForecast"/> or null.</returns>
        public async Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude)
        {
            return await this.GetForecastAsync(latitude, longitude);
        }

        /// <summary>
        /// Async method to get <see cref="City"/>  by city name.
        /// </summary>
        /// <param name="city">City name.</param>
        /// <returns><see cref="City"/>.</returns>
        public async Task<City?> GetCityByNameAsync(string city)
        {
            this.logger.LogInformation("Fetching '{City}' city from OpenMeteo Geocoding API.", city);

            // Composing parameterize request URL for Geocoding API endpoint.
            var builder = new UriBuilder(this.geocodingApiUrl);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["name"] = city.ToString();
            query["count"] = "1";
            query["language"] = "en";
            query["format"] = "json";
            builder.Query = query.ToString();
            string url = builder.ToString();

            // Calling the get method.
            var response = await this.GetAsync(url);
            if (response != null)
            {
                var geocoding = await JsonSerializer.DeserializeAsync<Geocoding>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
                if (geocoding != null && geocoding.Cities != null && geocoding.Cities.Length > 0)
                {
                    var cityModel = geocoding.Cities[0];
                    cityModel.SearchedByName = city.ToUpper();

                    this.logger.LogInformation("Fetching '{City}' city from OpenMeteo Geocoding API. City was found: {CityModel}", city, cityModel);
                    return cityModel;
                }
            }

            this.logger.LogInformation("Fetching '{City}' city from OpenMeteo Geocoding API. City not found!", city);
            return null;
        }

        private async Task<WeatherForecast?> GetForecastAsync(float latitude, float longitude, string timezone = "")
        {
            this.logger.LogInformation("Fetching coordinates {Latitude}, {Longitude} from OpenMeteo API.", latitude, longitude);

            // Composing parameterize request URL for Weather Forecast API endpoint.
            var builder = new UriBuilder(this.weatherApiUrl);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["latitude"] = latitude.ToString();
            query["longitude"] = longitude.ToString();
            query["current"] = "temperature_2m,wind_speed_10m,wind_direction_10m";
            query["daily"] = "temperature_2m_max,temperature_2m_min,sunrise,wind_speed_10m_max,wind_direction_10m_dominant";
            if (!string.IsNullOrEmpty(timezone))
            {
                query["timezone"] = timezone;
            }

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Calling the get method.
            var response = await this.GetAsync(url);
            if (response != null)
            {
                var weatherForecast = await JsonSerializer.DeserializeAsync<WeatherForecast>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (weatherForecast != null)
                {
                    weatherForecast.SearchedByLatitude = latitude;
                    weatherForecast.SearchedByLongitude = longitude;
                    weatherForecast.SearchedTimeStamp = DateTime.Now;
                }

                this.logger.LogInformation("Fetching coordinates {Latitude}, {Longitude} from OpenMeteo API. Location was found:{WeatherForecast}", latitude, longitude, weatherForecast);
                return weatherForecast;
            }

            this.logger.LogInformation("Fetching coordinates {Latitude}, {Longitude} from OpenMeteo API. Location not found!", latitude, longitude);
            return null;
        }

        /// <summary>
        /// Async method for making http get request to url.
        /// </summary>
        /// <param name="url">URL for request.</param>
        /// <returns>Returns a Stream or null.</returns>
        private async Task<Stream?> GetAsync(string url)
        {
            try
            {
                using var httpClient = this.httpClientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException e)
            {
                this.logger.LogError(e, "Error while fetching from OpenMeteo Api");
            }

            return null;
        }
    }
}
