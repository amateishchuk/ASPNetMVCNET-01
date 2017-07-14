using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.OwmService;
using WeatherApp.OwmService;

namespace WeatherApp.Tests.Fake
{
    public class FakeWeatherService : IWeatherService
    {
        public WeatherOwm GetWeatherInfo(string city, int qtyDays)
        {
            if (city == null || string.IsNullOrWhiteSpace(city) || string.IsNullOrEmpty(city))
                throw new ArgumentNullException();

            else if (qtyDays < 1 || qtyDays > 15)
                throw new ArgumentOutOfRangeException();            
            else
            {
                return new WeatherOwm
                {
                    City = new City { Name = city },
                    Cnt = qtyDays,
                    List = new List<DayData>() { new DayData() }
                };
            }
        }
    }
}
