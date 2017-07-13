using System;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.OwmService;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        readonly IWeatherService weatherService;
        readonly IUnitOfWork uow;

        public WeatherController(IWeatherService wService, IUnitOfWork uow)
        {
            weatherService = wService;
            this.uow = uow;
        }
        public ActionResult ShowWeather(string city = "Kiev", int qtyDays = 7)
        {
            try
            {
                var result = weatherService.GetWeatherInfo(city, qtyDays);
                var record = new HistoryRecord
                {
                    City = result.City.Name,
                    DateTime = DateTime.Now,
                    DayData = result.List[0],
                };

                if (uow.Repository<HistoryRecord>().Count > 14)
                {
                    var firstRecord = uow.Repository<HistoryRecord>().First;
                    uow.Repository<HistoryRecord>().Delete(firstRecord);
                }
                uow.Repository<HistoryRecord>().Insert(record);
                uow.SaveChanges();

                return View(result);
            }
            catch (ArgumentNullException)
            {
                return HttpNotFound("City name is incorrect");
            }
            catch (ArgumentOutOfRangeException)
            {
                return HttpNotFound("Quantity days must be in range beetwen 1 and 16");
            }            
        }
    }
}