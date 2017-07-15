using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherApp.Domain.Abstract;

namespace WeatherApp.Controllers.Api
{
    public class CityController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public HttpResponseMessage GetCities()
        {
            return Request.CreateResponse(HttpStatusCode.OK, 
                unitOfWork.Cities.GetAll());
        }
        public HttpResponseMessage Get(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city != null)
                return Request.CreateResponse(HttpStatusCode.OK, city);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest);

        }
        public HttpResponseMessage Get(string name)
        {
            var city = unitOfWork.Cities.Get(c => c.Name.ToLower() == name.ToLower());
            if (city != null)
                return Request.CreateResponse(HttpStatusCode.OK, city);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage PostCity(string name)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Cities.Insert(new Domain.Entities.City { Name = name });
                unitOfWork.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
        public HttpResponseMessage DeleteCity(int id)
        {
            var city = unitOfWork.Cities.Get(c => c.Id == id);
            if (city != null)
            {
                unitOfWork.Cities.Delete(id);
                unitOfWork.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
