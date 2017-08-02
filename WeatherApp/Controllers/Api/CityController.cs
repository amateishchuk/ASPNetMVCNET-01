using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> GetCities()
        {
            var cities = await unitOfWork.Cities.GetAllAsync();
            var result = Mapper.Map<IEnumerable<City>, IEnumerable<CityViewModel>>(cities);
            return Ok(result);
        }
        public async Task<IHttpActionResult> Get(int id)
        {
            var city = await unitOfWork.Cities.GetAsync(c => c.Id == id);
            if (city != null)
            {
                var result = Mapper.Map<City, CityViewModel>(city);
                return Ok(result);
            }
            else
                return BadRequest();
        }
        public async Task<IHttpActionResult> Get(string name)
        {
            var city = await unitOfWork.Cities.GetAsync(c => c.Name.ToLower() == name.ToLower());
            if (city != null)
            {
                var result = Mapper.Map<City, CityViewModel>(city);
                return Ok(result);
            }
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string city)
        {
            if (City.IsValidName(city))
            {
                await unitOfWork.Cities.InsertAsync(new City { Name = city });
                await unitOfWork.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest("Bad city name");
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody]City city)
        {
            if (ModelState.IsValid && await unitOfWork.Cities.GetAsync(c => c.Id == id) != null)
            {                
                unitOfWork.Cities.Update(city);
                await unitOfWork.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }
        public async Task<IHttpActionResult> Delete(int id)
        {
            var city = await unitOfWork.Cities.GetAsync(c => c.Id == id);
            if (city != null)
            {
                unitOfWork.Cities.Delete(city);
                await unitOfWork.SaveChangesAsync();
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
