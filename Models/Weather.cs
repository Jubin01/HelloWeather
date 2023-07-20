namespace HelloWeather.Models
{
    public class Weather
    {
        public Current? current { get; set; }
    }
    public class Current
    {
        public string? temp_c { get; set; }
    }

}