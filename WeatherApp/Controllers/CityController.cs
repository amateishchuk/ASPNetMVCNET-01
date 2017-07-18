using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers
{
    public class CityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CityController(IUnitOfWork unitOfWork)
        {
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
            if (City.IsValidName(city))
            {
                unitOfWork.Cities.Insert(new City { Name = city });
                unitOfWork.SaveChanges();
                return RedirectToAction("GetWeather", "Weather");
            }
            else
                return HttpNotFound("Bad city name");       
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
                return RedirectToAction("GetWeather", "Weather");
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
                return RedirectToAction("GetWeather", "Weather");
            }
            else
                return HttpNotFound("The city with specified ID doesn't exist");
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}