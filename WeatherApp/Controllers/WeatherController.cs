using System;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.OwmService;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService weatherService;
        private readonly IUnitOfWork unitOfWork;

        public WeatherController(IWeatherService wService, IUnitOfWork uow)
        {
            weatherService = wService;
            this.unitOfWork = uow;
        }
        public ActionResult ShowWeather(string city = "Kiev", int qtyDays = 7)
        {
            try
            {
                var result = weatherService.GetWeather(city, qtyDays);
                var record = new HistoryRecord(result);
                unitOfWork.History.Insert(record);
                unitOfWork.SaveChanges();

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
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            weatherService.Dispose();
            base.Dispose(disposing);
        }
    }
}