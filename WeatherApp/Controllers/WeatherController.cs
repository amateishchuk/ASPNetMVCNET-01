using System;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.OwmService;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        IWeatherService weatherService;
        IUnitOfWork db;

        public WeatherController(IWeatherService wService, IUnitOfWork dbService)
        {
            weatherService = wService;
            db = dbService;
            
        }
        public ActionResult ShowWeather(string city = "Kiev", int qtyDays = 7)
        {
            var result = weatherService.GetWeatherInfo(city, qtyDays);
            var record = new HistoryRecord
            {
                City = result.City.Name,
                DateTime = DateTime.Now,
                DayData = result.List[0],
            };
            db.HistoryRecords.Add(record);
            db.Save();

            return View(result);
        }
    }
}