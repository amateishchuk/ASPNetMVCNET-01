using AutoMapper;
using System.Collections.Generic;
using System.Web.Http;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Models;

namespace WeatherApp.Controllers.Api
{
    public class CityController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Mapper.Initialize(cfg => cfg.CreateMap<City, CityViewModel>());
        }
        public IHttpActionResult GetCities()
        {
            var result = Mapper.Map<IEnumerable<City>, IEnumerable<CityViewModel>>(unitOfWork.Cities.GetAll());
            return Ok(result);
        }
        public IHttpActionResult Get(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city != null)
            {
                var result = Mapper.Map<City, CityViewModel>(city);
                return Ok(result);
            }
            else
                return BadRequest();
        }
        public IHttpActionResult Get(string name)
        {
            var city = unitOfWork.Cities.Get(c => c.Name.ToLower() == name.ToLower());
            if (city != null)
            {
                var result = Mapper.Map<City, CityViewModel>(city);
                return Ok(result);
            }
            else
                return BadRequest();
        }
        [HttpPost]
        public IHttpActionResult Post([FromBody]string city)
        {
            if (City.IsValidName(city))
            {
                unitOfWork.Cities.Insert(new City { Name = city });
                unitOfWork.SaveChanges();
                return Ok();
            }
            else
                return BadRequest("Bad city name");
        }
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]City city)
        {
            if (ModelState.IsValid && unitOfWork.Cities.Get(c => c.Id == id) != null)
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
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
