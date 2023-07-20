namespace HelloWeather.Models
{
    public class MeteoStat
    {
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string? temp { get; set; }
    }
}