using System.Collections.Generic;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.OwmService;

namespace WeatherApp.OwmService
{

    public class WeatherOwm
    {
        public City City { get; set; }
        public string Cod { get; set; }
        public double Message { get; set; }
        public int Cnt { get; set; }
        public List<DayData> List { get; set; }
    }
}
