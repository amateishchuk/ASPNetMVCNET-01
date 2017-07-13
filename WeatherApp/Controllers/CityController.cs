using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers
{
    public class CityController : Controller
    {
        readonly IUnitOfWork uow;

        public CityController(IUnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }

        public ActionResult GetFavorites()
        {
            var cities = uow.Repository<City>().GetAll();
            return PartialView(cities);              
        }

        [HttpPost]
        public ActionResult Add(string city)
        {            
            city = city.Trim();

            if (string.IsNullOrEmpty(city))
                return HttpNotFound();

            var existCity = uow.Repository<City>().Find(c => c.Name == city);
            if (existCity == null)
            {
                uow.Repository<City>().Insert(new City { Name = city });
                uow.SaveChanges();
            }

            return RedirectToAction("ShowWeather", "Weather");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var city = uow.Repository<City>().Find(c => c.Id == id);
            if (city == null)
                return HttpNotFound();

            return View(city);    
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            uow.Repository<City>().Update(city);
            uow.SaveChanges();

            return RedirectToAction("ShowWeather", "Weather");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var city = uow.Repository<City>().Find(c => c.Id == id);
            if (city == null)
                return HttpNotFound();

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var city = uow.Repository<City>().Find(c => c.Id == id);
            if (city != null)
            {
                uow.Repository<City>().Delete(city);
                uow.SaveChanges();
            }

            return RedirectToAction("ShowWeather", "Weather");
        }
    }
}