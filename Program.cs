using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Formatting;
using Microsoft.Extensions.Logging;

namespace HelloWeather
{
    public interface IWeatherApi
    {
        public Task GetTodayWeather(HttpClient client, ILogger<Program> _logger, string path);
    }
    public class Current
    {
        public string? temp_c;
    }
    public class Weather
    {
        public Current? current { get; set; }
    }

    public class Program
    {
        static ILogger<Program> _logger;
        public Program(ILogger<Program> logger)
        {
            _logger = logger;
        }
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Task.Run(async () =>
       {
           WeatherApi wobj = new WeatherApi();
           await wobj.GetTodayWeather(client, _logger, "http://api.weatherapi.com/v1/current.json?key=fcac080ce36949789bd165809231907&q=India&aqi=no");
           MeteoStat mobj = new MeteoStat();
           await mobj.GetTodayWeather(client, _logger, "Not implemented");
       }).GetAwaiter().GetResult();

        }
    }

    public class WeatherApi : IWeatherApi
    {
        public async Task GetTodayWeather(HttpClient client, ILogger<Program> _logger, string path)
        {
            // using Stream stream = await client.GetStreamAsync(path);
            // var weath = await JsonSerializer.DeserializeAsync<Weather>(stream);
            // Console.WriteLine(weath?.current.temp_c);

            //foreach (var weath in weathers ?? Enumerable.Empty<Weather>())

            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                //var dataObjects = await JsonSerializer.DeserializeAsync<Weather>(response);
                var d = response.Content.ReadAsAsync<Weather>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                                                                         //foreach (var d in dataObjects)
                                                                         //{
                Console.WriteLine("Current temperature (weatherapi) - {0} degrees centigrade", d?.current.temp_c);
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);
                // }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
               // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);
            }
        }
    }

    public class MeteoStat : IWeatherApi
    {
        public async Task GetTodayWeather(HttpClient client, ILogger<Program> _logger, string path)
        {
            Console.WriteLine($"Current temperature (meteostat)- \"{path}\")");
        }
    }
}