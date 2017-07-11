using WeatherApp.OwmService;

namespace WeatherApp.Domain.Abstract
{
    public interface IWeatherService
    {
        WeatherOwm GetWeatherInfo(string city, int qtyDays);
    }
}
