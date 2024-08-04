using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using WeatherServiceAPI.Controllers;
using WeatherServiceAPI.Models;

namespace WeatherServiceAPI.Services
{
    public class OpenMeteoClientService
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _weatherApiUrl;
        private readonly string _geocodingApiUrl;
        public OpenMeteoClientService(ILogger<WeatherForecastController> logger,IHttpClientFactory httpClientFactory, IOptions<OpenMeteoClientSettings> openMeteoClientSettings)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _weatherApiUrl = openMeteoClientSettings.Value.WeatherApiUrl;
            _geocodingApiUrl = openMeteoClientSettings.Value.GeocodingApiUrl;
        }

        public async Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude)
        {
            return await GetForecastAsync(latitude, longitude);

        }

        public async Task<City?> GetCityByNameAsync(string city)
        {
            _logger.LogInformation($"Fetching {city} city from OpenMeteo Geocoding API");

            var builder = new UriBuilder(_geocodingApiUrl);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["name"] = city.ToString();
            query["count"] = "1";
            query["language"] = "en";
            query["format"] = "json";
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await GetAsync(url);
            if (response != null)
            {
                var geocoding = await JsonSerializer.DeserializeAsync<Geocoding>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
                if (geocoding != null && geocoding.Cities != null && geocoding.Cities.Length > 0)
                {
                    var cityModel = geocoding.Cities[0];
                    cityModel.SearchedByName = city.ToUpper();
                    return cityModel;
                }
            }
            return null;

        }

        private async Task<WeatherForecast?> GetForecastAsync(float latitude, float longitude, string timezone = "")
        {
            _logger.LogInformation($"Fetching coordinates {latitude.ToString()}, {longitude.ToString()} from OpenMeteo API");
            var builder = new UriBuilder(_weatherApiUrl);
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

            var response = await GetAsync(url);
            if(response !=null)
            {
                var weatherForecast = await JsonSerializer.DeserializeAsync<WeatherForecast>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if(weatherForecast != null)
                {
                    weatherForecast.SearchedByLatitude = latitude;
                    weatherForecast.SearchedByLongitude = longitude;
                    weatherForecast.SearchedTimeStamp = DateTime.Now;
                }
                return weatherForecast;
            }
            return null;
        }

        private async Task<Stream?> GetAsync(string url)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message, e.StackTrace);
            }
            return null;
        }
    }
}
