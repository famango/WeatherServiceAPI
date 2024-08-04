using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WeatherServiceAPI.Mappers;
using WeatherServiceAPI.Services;

namespace WeatherServiceAPI.Controllers
{
    [Route("api/weatherforecast")]
    public class WeatherForecastController : Controller
    {
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastController(WeatherForecastService weatherForcastService)
        {
            _weatherForecastService = weatherForcastService;
        }

        [HttpGet]
        [Route("bylocation")]
        public async Task<IActionResult> GetByLocation([FromQuery] float latitude, float longitude)
        {
            if (-90 > latitude || latitude > 90){
                ModelState.AddModelError("latitude", "Latitude is not a valid value.");
            }

            if ( -180 > longitude || longitude > 180)
            {
                ModelState.AddModelError("longitude", "Longitude is not a valid value.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _weatherForecastService.GetForecastByLocationAsync(latitude, longitude);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result.ToDto());
        }

        [HttpGet]
        [Route("bycity")]
        public async Task<IActionResult> GetByCity([FromQuery] string city)
        {
            int number;
            bool isParsedSuccessfully = Int32.TryParse(city, out number);

            if (isParsedSuccessfully)
            {
                ModelState.AddModelError("city", $"{city} is not a valid value for city.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _weatherForecastService.GetForecastByCityAsync(city.Trim());
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.ToDto());
        }
    }
}
