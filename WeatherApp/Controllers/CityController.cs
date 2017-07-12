using System.Linq;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers
{
    public class CityController : Controller
    {
        IUnitOfWork db;

        public CityController(IUnitOfWork dbService)
        {
            db = dbService;
        }

        public ActionResult GetFavorites()
        {
            return PartialView(db.Cities.GetAll());            
        }

        [HttpPost]
        public ActionResult Add(string city)
        {
            City newCity = new City { Name = city };

            db.Cities.Add(newCity);
            db.Save();

            return RedirectToAction("ShowWeather", "Weather");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(db.Cities.Get(id));
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            db.Cities.Update(city);
            db.Save();

            return RedirectToAction("ShowWeather", "Weather");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var city = db.Cities.Get(id);
            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            db.Cities.Delete(id);
            db.Save();
            return RedirectToAction("ShowWeather", "Weather");
        }
    }
}