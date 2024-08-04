namespace WeatherServiceAPI.Dtos
{
    public class WeatherForecastDto
    {
        public string Tempeture { get; set; } = string.Empty;
        public string WindDirection{ get; set; } = string.Empty;
        public string WindSpeed { get; set; } = string.Empty;
        public string Sunrise { get; set; } = string.Empty;
    }
}
