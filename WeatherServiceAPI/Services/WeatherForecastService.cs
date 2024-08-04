using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using WeatherServiceAPI.Models;

namespace WeatherServiceAPI.Services
{
    public class WeatherForecastService : IWeatherForecast
    {
        private readonly OpenMeteoClientService _openMeteoClientService;
        private readonly MongoDBService _mongoDBService;
        private readonly int _weatherForecastDbExpiresMintues;
        public WeatherForecastService(OpenMeteoClientService openMeteoClientService, MongoDBService mongoDBService, IOptions<WeatherForecastSettings> weatherForecastSettgins)
        {
            _openMeteoClientService = openMeteoClientService;
            _mongoDBService = mongoDBService;
            _weatherForecastDbExpiresMintues = weatherForecastSettgins.Value.WeatherForecastDbExpiresMintues;
        }

        public async Task<WeatherForecast?> GetForecastByCityAsync(string city)
        {
            City? cityModel;
            cityModel = await  _mongoDBService.GetCityByNameAsync(city);
            if(cityModel == null)
            {
                cityModel = await _openMeteoClientService.GetCityByNameAsync(city);
                if (cityModel != null)
                {
                    await _mongoDBService.CreateCityAsync(cityModel);
                }
            }
   
            if (cityModel != null)
            {
                return await GetForecastAsync(cityModel.Latitude, cityModel.Longitude);
            }
            return null;
        }

        public async  Task<WeatherForecast?> GetForecastByLocationAsync(float latitude, float longitude)
        {
            return await GetForecastAsync(latitude, longitude);
        }

        private async Task<WeatherForecast?> GetForecastAsync(float latitude, float longitude)
        {
            var weatherForcast = await _mongoDBService.GetRecentWeatherForecastByLocationAsync(latitude, longitude, _weatherForecastDbExpiresMintues);
            if(weatherForcast == null)
            {
                weatherForcast = await _openMeteoClientService.GetForecastByLocationAsync(latitude, longitude);
                if (weatherForcast != null)
                {
                    await _mongoDBService.CreateWeatherForecastAsync(weatherForcast);
                }
            }
            return weatherForcast;
        }
    }
}
