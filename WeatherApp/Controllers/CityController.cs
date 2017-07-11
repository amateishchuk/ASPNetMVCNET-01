using System.Linq;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers
{
    public class CityController : Controller
    {
        IRepository repository;

        public CityController(IRepository repo)
        {
            repository = repo;
        }

        public ActionResult GetFavorites(int qtyDays)
        {
            return PartialView(repository.FavoriteCities);            
        }

        [HttpPost]
        public ActionResult Add(string city)
        {
            repository.AddToFavorites(city);

            return RedirectToAction("ShowWeather", "Weather");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(repository.GetCityById(id));
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            repository.UpdateExists(city);
            return RedirectToAction("ShowWeather", "Weather");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var city = repository.GetCityById(id);
            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            repository.DeleteFromFavorites(id);
            return RedirectToAction("ShowWeather", "Weather");
        }
    }
}