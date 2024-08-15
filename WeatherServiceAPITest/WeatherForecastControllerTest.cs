using Amazon.Runtime.Internal.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherServiceAPI.Controllers;
using WeatherServiceAPI.Dtos;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Services;

namespace WeatherServiceAPITest
{
    public class WeatherForecastControllerTest
    {
        private WeatherForecastDto dto;
        private WeatherForecast? forecast;
        private WeatherForecast? nullForecast = null;
        [SetUp]
        public void Setup()
        {
            dto = new() {
                Tempeture = "39.2 °C.",
                WindDirection = "359 °",
                WindSpeed = "15.8 km/h",
                Sunrise = "2024-08-15T12:31",
            };

            forecast = new(){
                Id = "66be1a1da778a5337243bd00",
                Latitude = 31.768824F,
                Longitude = -106.503784F,
                SearchedByLatitude = 31.75872F,
                SearchedByLongitude = -106.48693F,
                SearchedTimeStamp = DateTime.Parse("2024-08-15T15:09:17.702Z"),
                GenerationTimeMs = 0.13899803F,
                UtcOffsetSeconds = 0,
                Timezone = "GMT",
                TimezoneAbbreviation = "GMT",
                Daily = new Daily(){
                    TemperatureMax = new float[] { 39.2F, 40.2F, 39.2F, 37.7F, 37.2F, 36.7F, 37.5F },
                    TemperatureMin = new float[] { 25.4F, 24.2F, 25, 28, 27.7F, 26.9F, 28.2F },
                    Sunrise = new string[] {
                      "2024-08-15T12:31",
                      "2024-08-16T12:32",
                      "2024-08-17T12:32",
                      "2024-08-18T12:33",
                      "2024-08-19T12:34",
                      "2024-08-20T12:34",
                      "2024-08-21T12:35"
                    },
                    WindSpeed = new float[] { 15.8F, 11.5F, 29, 32.4F, 29.4F, 28.7F, 21.3F },
                    WindDirection = new float[] { 359, 88, 90, 109, 115, 118, 131 },
                 },
                DailyUnits = new DailyUnits (){
                    Sunrise = "iso8601",
                    TemperatureMax = "°C",
                    TemperatureMin = "°C",
                    WindspeedMax = "km/h",
                    WindDirection = "°"
                  }
            };
        }

        [Test]
        public async Task Given_ValidInput_When_GetByLocation_Then_ReturnResult() {
            // Arrange
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var weatherForecastService = new Mock<IWeatherForecastService>();
            weatherForecastService.Setup(m => m.GetForecastByLocationAsync(latitude, longitude)).ReturnsAsync(forecast);
            var logerFactory = new NullLoggerFactory();//new Mock<ILoggerFactory<IWeatherForecastController>();
            var weatherForecastController = new WeatherForecastController(weatherForecastService.Object, logerFactory);

            // Act
            var actionResult = await weatherForecastController.GetByLocation(latitude, longitude);

            // Assert
            weatherForecastService.Verify(m => m.GetForecastByLocationAsync(latitude, longitude), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
            var okResult = (OkObjectResult) actionResult;
            Assert.IsInstanceOf<WeatherForecastDto>(okResult.Value);
            WeatherForecastDto returnValue = new WeatherForecastDto();
            if (okResult.Value != null)
                returnValue = (WeatherForecastDto)okResult.Value;
            var expectedJson = JsonSerializer.Serialize(dto);
            var actualJson = JsonSerializer.Serialize(returnValue);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        [Test]
        public async Task Given_ValidInputNotFound_When_GetByLocation_Then_ReturnNotFound()
        {
            // Arrange
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var weatherForecastService = new Mock<IWeatherForecastService>();
            weatherForecastService.Setup(m => m.GetForecastByLocationAsync(latitude, longitude)).ReturnsAsync(nullForecast);
            var logerFactory = new NullLoggerFactory();//new Mock<ILoggerFactory<IWeatherForecastController>();
            var weatherForecastController = new WeatherForecastController(weatherForecastService.Object, logerFactory);

            // Act
            var actionResult = await weatherForecastController.GetByLocation(latitude, longitude);

            // Assert
            weatherForecastService.Verify(m => m.GetForecastByLocationAsync(latitude, longitude), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public async Task Given_InvalidInputNotFound_When_GetByLocation_Then_ReturnBadRequest()
        {
            // Arrange
            SerializableError expected = new SerializableError();
            expected.Add("latitude", new string[] { "Latitude is not a valid value." });
            expected.Add("longitude", new string[] { "Longitude is not a valid value." });
            float latitude = 1000.001F;
            float longitude = -1000.001F;
            var weatherForecastService = new Mock<IWeatherForecastService>();
            weatherForecastService.Setup(m => m.GetForecastByLocationAsync(latitude, longitude)).ReturnsAsync(nullForecast);
            var logerFactory = new NullLoggerFactory();//new Mock<ILoggerFactory<IWeatherForecastController>();
            var weatherForecastController = new WeatherForecastController(weatherForecastService.Object, logerFactory);

            // Act
            var actionResult = await weatherForecastController.GetByLocation(latitude, longitude);

            // Assert
            weatherForecastService.Verify(m => m.GetForecastByLocationAsync(It.IsAny<float>(), It.IsAny<float>()), Times.Never);
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            Assert.IsInstanceOf<SerializableError>(badRequestResult.Value);
            SerializableError returnValue = new SerializableError();
            if (badRequestResult.Value != null)
                returnValue = (SerializableError)badRequestResult.Value;
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(returnValue);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }


        [Test]
        public async Task Given_ValidInput_When_GetByCity_Then_ReturnResult()
        {
            // Arrange
            string city = "El Paso";
            var weatherForecastService = new Mock<IWeatherForecastService>();
            weatherForecastService.Setup(m => m.GetForecastByCityAsync(city)).ReturnsAsync(forecast);
            var logerFactory = new NullLoggerFactory();//new Mock<ILoggerFactory<IWeatherForecastController>();
            var weatherForecastController = new WeatherForecastController(weatherForecastService.Object, logerFactory);

            // Act
            var actionResult = await weatherForecastController.GetByCity(city);

            // Assert
            weatherForecastService.Verify(m => m.GetForecastByCityAsync(city), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
            var okResult = (OkObjectResult)actionResult;
            Assert.IsInstanceOf<WeatherForecastDto>(okResult.Value);
            WeatherForecastDto returnValue = new WeatherForecastDto();
            if (okResult.Value != null)
                returnValue = (WeatherForecastDto)okResult.Value;
            var expectedJson = JsonSerializer.Serialize(dto);
            var actualJson = JsonSerializer.Serialize(returnValue);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        [Test]
        public async Task Given_ValidInputNotFound_When_GetByCity_Then_ReturnNotFound()
        {
            // Arrange
            string city = "El Paso";
            var weatherForecastService = new Mock<IWeatherForecastService>();
            weatherForecastService.Setup(m => m.GetForecastByCityAsync(city)).ReturnsAsync(nullForecast);
            var logerFactory = new NullLoggerFactory();//new Mock<ILoggerFactory<IWeatherForecastController>();
            var weatherForecastController = new WeatherForecastController(weatherForecastService.Object, logerFactory);

            // Act
            var actionResult = await weatherForecastController.GetByCity(city);

            // Assert
            weatherForecastService.Verify(m => m.GetForecastByCityAsync(city), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public async Task Given_InvalidInputNotFound_When_GetByCity_Then_ReturnBadRequest()
        {
            // Arrange
            SerializableError expected = new SerializableError();
            expected.Add("city", new string[] { "32575 is not a valid value for city." });
            string city = "32575";
            var weatherForecastService = new Mock<IWeatherForecastService>();
            weatherForecastService.Setup(m => m.GetForecastByCityAsync(city)).ReturnsAsync(nullForecast);
            var logerFactory = new NullLoggerFactory();//new Mock<ILoggerFactory<IWeatherForecastController>();
            var weatherForecastController = new WeatherForecastController(weatherForecastService.Object, logerFactory);

            // Act
            var actionResult = await weatherForecastController.GetByCity(city);

            // Assert
            weatherForecastService.Verify(m => m.GetForecastByCityAsync(It.IsAny<string>()), Times.Never);
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            Assert.IsInstanceOf<SerializableError>(badRequestResult.Value);
            SerializableError returnValue = new SerializableError();
            if (badRequestResult.Value != null)
                returnValue = (SerializableError)badRequestResult.Value;
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(returnValue);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }
    }
}
