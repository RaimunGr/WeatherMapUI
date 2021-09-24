using MessageQueuing.Implementations;
using Newtonsoft.Json;
using System;
using WeatherMapUI.Models;

namespace WeatherMapUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var puller = new MessagePuller("amqp://guest:guest@localhost:5672");
            puller.Pull("WeatherMap_Queue", (headers, jsonData) =>
            {
                var weatherMap = JsonConvert.DeserializeObject<WeatherMap>(jsonData);
                var indentedJson = JsonConvert.SerializeObject(weatherMap, Formatting.Indented);
                Console.WriteLine(indentedJson);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("———————————————————————————————————————————");
                Console.ForegroundColor = ConsoleColor.White;
            });

            Console.ReadKey();
        }
    }
}
