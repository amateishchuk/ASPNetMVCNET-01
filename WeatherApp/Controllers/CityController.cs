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

        public PartialViewResult GetFavorites()
        {
            var cities = uow.Cities.GetAll();
            return PartialView(cities);              
        }

        [HttpPost]
        public ActionResult Add(string city)
        {            
            city = city.Trim();

            if (string.IsNullOrEmpty(city))
                return HttpNotFound();

            uow.Cities.Insert(new City { Name = city });
            uow.SaveChanges();

            return RedirectToAction("ShowWeather", "Weather");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var city = uow.Cities.Get(id);
            if (city == null)
                return HttpNotFound("City with specified ID doesn't exist");

            return View(city);    
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            uow.Cities.Update(city);
            uow.SaveChanges();

            return RedirectToAction("ShowWeather", "Weather");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var city = uow.Cities.Get(id);
            if (city == null)
                return HttpNotFound();

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            uow.Cities.Delete(id);
            uow.SaveChanges();          

            return RedirectToAction("ShowWeather", "Weather");
        }
    }
}