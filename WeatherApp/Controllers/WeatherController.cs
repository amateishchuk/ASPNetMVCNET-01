using System;
using System.Net.Http;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

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
        public ActionResult GetWeather(string city = "Kiev", int qtyDays = 7)
        {
            try
            {
                var result = weatherService.GetWeather(city, qtyDays);
                var record = new HistoryRecord(result);
                unitOfWork.History.Insert(record);
                unitOfWork.SaveChanges();

                return View(result);
            }
            catch (ArgumentOutOfRangeException)
            {
                return HttpNotFound("Quantity days must be between 1 and 16");
            }
            catch (ArgumentNullException)
            {
                return HttpNotFound("City can't be whitespaces or empty");
            }
            catch (ArgumentException)
            {
                return HttpNotFound("City can't be null");
            }
            catch (AggregateException)
            {
                return HttpNotFound("Error occured");
            }
            catch (HttpRequestException)
            {
                return HttpNotFound("Bad city name");
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