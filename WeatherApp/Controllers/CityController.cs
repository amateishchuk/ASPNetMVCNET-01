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
            if (ModelState.IsValid)
            {
                var result = weatherService.GetWeather(city, 1);
                var newCity = result.City;
                newCity.Id = 0;

                unitOfWork.Cities.Insert(newCity);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("ShowWeather", "Weather");            
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city == null)
                return HttpNotFound("City with specified ID doesn't exist");

            return View(city);    
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            unitOfWork.Cities.Update(city);
            unitOfWork.SaveChanges();

            return RedirectToAction("ShowWeather", "Weather");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city == null)
                return HttpNotFound();

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            unitOfWork.Cities.Delete(id);
            unitOfWork.SaveChanges();          

            return RedirectToAction("ShowWeather", "Weather");
        }
        protected override void Dispose(bool disposing)
        {
            weatherService.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}