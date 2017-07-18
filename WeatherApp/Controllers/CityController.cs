using System;
using System.Net.Http;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers
{
    public class CityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWeatherService weatherService;

        public CityController(IUnitOfWork unitOfWork, IWeatherService weatherService)
        {
            this.weatherService = weatherService;
            this.unitOfWork = unitOfWork;
        }

        public PartialViewResult GetFavorites()
        {
            var cities = unitOfWork.Cities.GetAll();
            return PartialView(cities);              
        }

        [HttpPost]
        public ActionResult Add(string city)
        {
            try
            {
                var result = weatherService.GetWeather(city, 1);
                var newCity = result.City;
                newCity.Id = 0;

                unitOfWork.Cities.Insert(newCity);
                unitOfWork.SaveChanges();
                return RedirectToAction("ShowWeather", "Weather");
            }
            catch (ArgumentNullException)
            {
                return HttpNotFound("City can't be whitespaces or empty");
            }
            catch (ArgumentException)
            {
                return HttpNotFound("City can't be nul");
            }
            catch (AggregateException)
            {
                return HttpNotFound("Bad city name");
            }
            catch (HttpRequestException)
            {
                return HttpNotFound("Bad city name");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city == null)
                return HttpNotFound("The city with specified ID doesn't exist");

            return View(city);    
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            if (ModelState.IsValid && unitOfWork.Cities.Get(c => c.Id == city.Id) != null)
            {
                unitOfWork.Cities.Update(city);
                unitOfWork.SaveChanges();
                return RedirectToAction("ShowWeather", "Weather");
            }
            return
                HttpNotFound("The city with specified ID doesn't exist");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city == null)
                return HttpNotFound("The city with specified ID doesn't exist");

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city != null)
            {
                unitOfWork.Cities.Delete(city);
                unitOfWork.SaveChanges();
                return RedirectToAction("ShowWeather", "Weather");
            }
            else
                return HttpNotFound("The city with specified ID doesn't exist");
        }
        protected override void Dispose(bool disposing)
        {
            weatherService.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}