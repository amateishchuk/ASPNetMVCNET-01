using System;
using System.Threading.Tasks;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Abstract
{
    public interface IWeatherService : IDisposable
    {
        WeatherOwm GetWeather(string city, int qtyDays);
        Task<WeatherOwm> GetWeatherAsync(string city, int qtyDays);
    }
}
