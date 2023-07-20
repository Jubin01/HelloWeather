using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Formatting;
using HelloWeather.Models;
using Microsoft.Extensions.Logging;
using NLog;

namespace HelloWeather
{
    public interface IWeatherApi
    {
        public Task GetTodayWeather(HttpClient client, Logger _logger, string path);
    }
    public class Program
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Task.Run(async () =>
       {

           IWeatherApi wobj = new WeatherApi();
           await wobj.GetTodayWeather(client, _logger, "http://api.weatherapi.com/v1/current.json?key=fcac080ce36949789bd165809231907&q=India&aqi=no");
           IWeatherApi mobj = new MeteoStatApi();
           await mobj.GetTodayWeather(client, _logger, "https://meteostat.p.rapidapi.com/stations/hourly?station=10637&start=2020-01-01&end=2020-01-01&tz=GMT");
       }).GetAwaiter().GetResult();

        }
    }

    public class WeatherApi : IWeatherApi
    {
        public async Task GetTodayWeather(HttpClient client, Logger _logger, string path)
        {
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                var d = response.Content.ReadAsAsync<Weather>().Result;
                Console.WriteLine("Current temperature (weatherapi) - {0} degrees centigrade", d?.current.temp_c);
                // _logger.Warn("Worker running at: {time}", DateTimeOffset.UtcNow);
                //using Streamwriter to log to file
                using (StreamWriter writetext = new StreamWriter("c:\\Logs\\write.txt", true))
                {
                    writetext.WriteLine("Current temperature (weatherapi) - {0} degrees centigrade", d?.current.temp_c);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);
            }
        }
    }

    public class MeteoStatApi : IWeatherApi
    {
        public async Task GetTodayWeather(HttpClient client, Logger _logger, string path)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(path),
                Headers =
    {
        { "X-RapidAPI-Key", "d070686c55msh51acbfecc10ae06p178c63jsn63c4051f4f8c" },
        { "X-RapidAPI-Host", "meteostat.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var d = response.Content.ReadAsAsync<MeteoStat>().Result;
                Console.WriteLine("Current temperature (meteostat) - {0}", d.data[0].temp);
                //using Streamwriter to log to file
                using (StreamWriter writetext = new StreamWriter("c:\\Logs\\write.txt", true))
                {
                    writetext.WriteLine("Current temperature (meteostat) - {0}", d.data[0].temp);
                }
            }
        }
    }
}