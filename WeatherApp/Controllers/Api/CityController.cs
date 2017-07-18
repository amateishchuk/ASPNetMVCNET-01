using System;
using System.Net.Http;
using System.Web.Http;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers.Api
{
    public class CityController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWeatherService weatherService;

        public CityController(IUnitOfWork unitOfWork, IWeatherService weatherService)
        {
            this.weatherService = weatherService;
            this.unitOfWork = unitOfWork;
        }
        public IHttpActionResult GetCities()
        {
            return Ok(unitOfWork.Cities.GetAll());
        }
        public IHttpActionResult Get(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city != null)
                return Ok(city);
            else
                return BadRequest();

        }
        public IHttpActionResult Get(string name)
        {
            var city = unitOfWork.Cities.Get(c => c.Name.ToLower() == name.ToLower());
            if (city != null)
                return Ok(city);
            else
                return BadRequest();
        }

        public IHttpActionResult Post(string name)
        {           
            try
            {
                var result = weatherService.GetWeather(name, 1);
                var city = result.City;
                city.Id = 0;

                unitOfWork.Cities.Insert(city);
                unitOfWork.SaveChanges();
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("City can't be whitespaces or empty");
            }
            catch (ArgumentException)
            {
                return BadRequest("City can't be nul");
            }
            catch (AggregateException)
            {
                return BadRequest("Bad city name");
            }
            catch (HttpRequestException)
            {
                return BadRequest("Bad city name");
            }            
        }
        public IHttpActionResult Put(int id, City city)
        {
            if (ModelState.IsValid && city.Id == id && unitOfWork.Cities.Get(c => c.Id == id) != null)
            {                
                unitOfWork.Cities.Update(city);
                unitOfWork.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }
        public IHttpActionResult Delete(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city != null)
            {
                unitOfWork.Cities.Delete(city);
                unitOfWork.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        protected override void Dispose(bool disposing)
        {
            weatherService.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
