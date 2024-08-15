using Microsoft.Extensions.Options;
using Moq;
using System.Xml.Linq;
using System;
using WeatherServiceAPI.Models;
using WeatherServiceAPI.Services;

namespace WeatherServiceAPITest
{
    public class WeatherForecastServiceTest
    {
        private City? city;
        private WeatherForecast? forecast;
        private City? nullCity = null;
        private WeatherForecast? nullForecast = null;

        [SetUp]
        public void Setup()
        {
            city = new()
            {
                CityId = 5520993,
                Name = "El Paso",
                SearchedByName = "EL PASO",
                Latitude = 31.75872039794922F,
                Longitude = -106.48693084716797F,
                Timezone = "America/Denver"
            };

            forecast = new()
            {
                Latitude = 31.75872039794922F,
                Longitude = -106.48693084716797F,
                SearchedByLatitude = 31.75872039794922F,
                SearchedByLongitude = -106.48693084716797F, 
            };
        }

        [Test]
        public async Task Given_ValidInputFoundInDB_When_GetForecastByCityAsync_Then_ReturnWeatherForecastFromDB()
        {
            // Arrange
            var cityName = "El Paso";
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var openMeteoClientService = new Mock<IOpenMeteoClientService>();
            var mongoDBService = new Mock<IMongoDBService>();
            var weatherForecastSettings = Options.Create(new WeatherForecastSettings());
            mongoDBService.Setup(m => m.GetCityByNameAsync(cityName)).ReturnsAsync(city);// City name found in database.
            mongoDBService.Setup(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues)).ReturnsAsync(forecast);
            openMeteoClientService.Setup(m => m.GetCityByNameAsync(cityName)).ReturnsAsync(nullCity);
            openMeteoClientService.Setup(m => m.GetForecastByLocationAsync(latitude, longitude)).ReturnsAsync(nullForecast);
            var weatherForecastService = new WeatherForecastService(openMeteoClientService.Object,mongoDBService.Object, weatherForecastSettings);
            
            // Act
            var weatherForecast = await weatherForecastService.GetForecastByCityAsync(cityName);

            // Assert
            mongoDBService.Verify(m => m.GetCityByNameAsync(cityName), Times.Once);
            mongoDBService.Verify(m => m.CreateCityAsync(It.IsAny<City>()), Times.Never);
            mongoDBService.Verify(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues), Times.Once);
            Assert.That(weatherForecast, Is.EqualTo(forecast));
        }

        [Test]
        public async Task Given_ValidInputNotFoundInDB_When_GetForecastByCityAsync_Then_CreateCityFromOpenMeteo()
        {
            // Arrange
            var cityName = "El Paso";
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var openMeteoClientService = new Mock<IOpenMeteoClientService>();
            var mongoDBService = new Mock<IMongoDBService>();
            var weatherForecastSettings = Options.Create(new WeatherForecastSettings());
            mongoDBService.Setup(m => m.GetCityByNameAsync(cityName)).ReturnsAsync(nullCity); // Returning null from db.
            mongoDBService.Setup(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues)).ReturnsAsync(forecast);
            if(city != null)
                mongoDBService.Setup(m => m.CreateCityAsync(city));
            openMeteoClientService.Setup(m => m.GetCityByNameAsync(cityName)).ReturnsAsync(city); // City fetched from OpenMeteo api.
            openMeteoClientService.Setup(m => m.GetForecastByLocationAsync(latitude, longitude)).ReturnsAsync(nullForecast);
            var weatherForecastService = new WeatherForecastService(openMeteoClientService.Object, mongoDBService.Object, weatherForecastSettings);

            // Act
            var weatherForecast = await weatherForecastService.GetForecastByCityAsync(cityName);

            // Assert
            mongoDBService.Verify(m => m.GetCityByNameAsync(cityName), Times.Once);
            mongoDBService.Verify(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues), Times.Once);
            if (city != null)
                mongoDBService.Verify(m => m.CreateCityAsync(city), Times.Once);
            openMeteoClientService.Verify(m => m.GetCityByNameAsync(cityName), Times.Once);
            Assert.That(weatherForecast, Is.EqualTo(forecast));
        }

        [Test]
        public async Task Given_ValidInputNotFound_When_GetForecastByCityAsync_Then_ReturnNull()
        {
            // Arrange
            var cityName = "El Paso";
            var openMeteoClientService = new Mock<IOpenMeteoClientService>();
            var mongoDBService = new Mock<IMongoDBService>();
            var weatherForecastSettings = Options.Create(new WeatherForecastSettings());
            mongoDBService.Setup(m => m.GetCityByNameAsync(cityName)).ReturnsAsync(nullCity); // Returning null from DB.
            openMeteoClientService.Setup(m => m.GetCityByNameAsync(cityName)).ReturnsAsync(nullCity); // Returning null from OpenMeteo.
            var weatherForecastService = new WeatherForecastService(openMeteoClientService.Object, mongoDBService.Object, weatherForecastSettings);

            // Act
            var weatherForecast = await weatherForecastService.GetForecastByCityAsync(cityName);

            // Assert
            mongoDBService.Verify(m => m.GetCityByNameAsync(cityName), Times.Once);
            mongoDBService.Verify(m => m.CreateCityAsync(It.IsAny<City>()), Times.Never);
            openMeteoClientService.Verify(m => m.GetCityByNameAsync(cityName), Times.Once);
            Assert.That(weatherForecast, Is.EqualTo(nullForecast));
        }

        [Test]
        public async Task Given_ValidInputFoundInDB_When_GetForecastByLocationAsync_Then_ReturnWeatherForecastFromDB()
        {
            // Arrange
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var openMeteoClientService = new Mock<IOpenMeteoClientService>();
            var mongoDBService = new Mock<IMongoDBService>();
            var weatherForecastSettings = Options.Create(new WeatherForecastSettings());
            mongoDBService.Setup(m => 
                m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues))
                .ReturnsAsync(forecast); //Returning record from db.
            openMeteoClientService.Setup(m => m.GetForecastByLocationAsync(latitude, longitude)).ReturnsAsync(nullForecast);
            var weatherForecastService = new WeatherForecastService(openMeteoClientService.Object, mongoDBService.Object, weatherForecastSettings);

            // Act
            var weatherForecast = await weatherForecastService.GetForecastByLocationAsync(latitude, longitude);

            // Assert
            mongoDBService.Verify(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues), Times.Once);
            openMeteoClientService.Verify(m => m.GetForecastByLocationAsync(It.IsAny<float>(), It.IsAny<float>()), Times.Never);
            Assert.That(weatherForecast, Is.EqualTo(forecast));
        }

        [Test]
        public async Task Given_ValidInputNotFoundInDB_When_GetForecastByLocationAsync_Then_ReturnWeatherForecastFromOpenMeteo()
        {
            // Arrange
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var openMeteoClientService = new Mock<IOpenMeteoClientService>();
            var mongoDBService = new Mock<IMongoDBService>();
            var weatherForecastSettings = Options.Create(new WeatherForecastSettings());
            mongoDBService.Setup(m => 
                m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues))
                .ReturnsAsync(nullForecast); // Returning null from db.
            if (forecast != null)
                mongoDBService.Setup(m => m.CreateWeatherForecastAsync(forecast));
            openMeteoClientService.Setup(m => 
                m.GetForecastByLocationAsync(latitude, longitude))
                .ReturnsAsync(forecast); //Returning forcast from OpenMeteo.
            var weatherForecastService = new WeatherForecastService(openMeteoClientService.Object, mongoDBService.Object, weatherForecastSettings);

            // Act
            var weatherForecast = await weatherForecastService.GetForecastByLocationAsync(latitude, longitude);

            // Assert
            mongoDBService.Verify(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues), Times.Once);
            if (forecast != null)
                mongoDBService.Verify(m => m.CreateWeatherForecastAsync(forecast), Times.Once);
            openMeteoClientService.Verify(m => m.GetForecastByLocationAsync(latitude, longitude), Times.Once);
            Assert.That(weatherForecast, Is.EqualTo(forecast));
        }

        [Test]
        public async Task Given_ValidInputNotNotFound_When_GetForecastByLocationAsync_Then_ReturnNull()
        {
            // Arrange
            float latitude = 31.75872039794922F;
            float longitude = -106.48693084716797F;
            var openMeteoClientService = new Mock<IOpenMeteoClientService>();
            var mongoDBService = new Mock<IMongoDBService>();
            var weatherForecastSettings = Options.Create(new WeatherForecastSettings());
            mongoDBService.Setup(m =>
                m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues))
                .ReturnsAsync(nullForecast); // Returning null from db.
            openMeteoClientService.Setup(m =>
                m.GetForecastByLocationAsync(latitude, longitude))
                .ReturnsAsync(nullForecast); //Returning null from OpenMeteo.
            var weatherForecastService = new WeatherForecastService(openMeteoClientService.Object, mongoDBService.Object, weatherForecastSettings);

            // Act
            var weatherForecast = await weatherForecastService.GetForecastByLocationAsync(latitude, longitude);

            // Assert
            mongoDBService.Verify(m => m.GetRecentWeatherForecastByLocationAsync(latitude, longitude, weatherForecastSettings.Value.WeatherForecastDbExpiresMintues), Times.Once);
            openMeteoClientService.Verify(m => m.GetForecastByLocationAsync(latitude, longitude), Times.Once);
            Assert.That(weatherForecast, Is.EqualTo(nullForecast));
        }
    }
}