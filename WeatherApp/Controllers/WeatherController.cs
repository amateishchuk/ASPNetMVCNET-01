using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Services;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Abstract;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        IWeather weatherService;

        public WeatherController(IWeather wService)
        {
            weatherService = wService;
        }
        public ActionResult ShowWeather(string city = "Kiev", int qtyDays = 1)
        {
            WeatherOwm result = weatherService.GetWeatherInfo(city, qtyDays);
            return View(result);
        }
    }
}