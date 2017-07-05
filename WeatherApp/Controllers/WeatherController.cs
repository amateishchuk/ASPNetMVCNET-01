using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Services;
using WeatherApp.Domain.Concrete;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        ServiceOWM service;

        public WeatherController()
        {
            service = new ServiceOWM();
        }
        public ActionResult ShowWeather(string city = "Kiev", int qtyDays = 1)
        {
            WeatherOWM result = service.GetWeatherInfo(city, qtyDays);
            return View(result);
        }
    }
}