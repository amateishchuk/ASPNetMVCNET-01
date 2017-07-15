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
            city = city.Trim();

            if (string.IsNullOrEmpty(city))
                return HttpNotFound();

            unitOfWork.Cities.Insert(new City { Name = city });
            unitOfWork.SaveChanges();

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
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}