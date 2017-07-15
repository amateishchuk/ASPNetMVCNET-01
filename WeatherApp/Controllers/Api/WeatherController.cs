using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherApp.Domain.Abstract;

namespace WeatherApp.Controllers.Api
{
    public class WeatherController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWeatherService weatherService;

        public WeatherController(IUnitOfWork unitOfWork, IWeatherService weatherService)
        {
            this.unitOfWork = unitOfWork;
            this.weatherService = weatherService;
        }

        public HttpResponseMessage GetWeather(string city, int qtyDays)
        {
            var result = weatherService.GetWeatherInfo(city, qtyDays);
            unitOfWork.History.Insert(new Domain.Entities.HistoryRecord(result));
            unitOfWork.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            weatherService.Dispose();
            base.Dispose(disposing);
        }
    }
}
