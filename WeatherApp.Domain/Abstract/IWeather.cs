using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Concrete;

namespace WeatherApp.Domain.Abstract
{
    public interface IWeather
    {
        WeatherOwm GetWeatherInfo(string city, int qtyDays);
    }
}
