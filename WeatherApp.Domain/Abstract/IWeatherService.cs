using System;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Abstract
{
    public interface IWeatherService : IDisposable
    {
        WeatherOwm GetWeather(string city, int qtyDays);
    }
}
