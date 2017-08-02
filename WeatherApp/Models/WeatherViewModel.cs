using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Domain.Entities;
using WeatherApp.OwmService;

namespace WeatherApp.Models
{
    public class WeatherViewModel
    {
        public WeatherOwm Weather { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }
}