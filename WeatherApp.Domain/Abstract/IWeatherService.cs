using System;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Abstract
{
    public interface IWeatherService : IDisposable
    {
        WeatherOwm GetWeatherInfo(string city, int qtyDays);
    }
}
