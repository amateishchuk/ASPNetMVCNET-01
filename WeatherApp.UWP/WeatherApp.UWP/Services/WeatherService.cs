using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.UWP.Models;

namespace WeatherApp.UWP.Services
{
    public class WeatherService
    {
        private string url = "http://localhost:8684/api/weather";

        public async Task<Weather> GetWeather(string city, int qtyDays = 7)
        {
            string json = null;

            using (var client = new HttpClient())
            {
                json = await client.GetStringAsync($"{this.url}/{city}/{qtyDays}");
            }

            return JsonConvert.DeserializeObject<Weather>(json);
        }
    }
}
