using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.UWP.Models;

namespace WeatherApp.UWP.Services
{
    public class WeatherService
    {
        private string url = "http://localhost:8684/api/weather";

        public async Task<Weather> GetWeather(string city, int qtyDays)
        {
            string json = null;

            using (var client = new HttpClient())
            {
                json = await client.GetStringAsync($"{url}/{city}/{qtyDays}");
            }

            return JsonConvert.DeserializeObject<Weather>(json);
        }
    }
}
