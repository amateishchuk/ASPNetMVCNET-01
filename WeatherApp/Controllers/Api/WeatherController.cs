using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<IHttpActionResult> GetWeather(string city, int qtyDays)
        {
            try
            {
                var result = await weatherService.GetWeatherAsync(city, qtyDays);
                await unitOfWork.History.InsertAsync(new Domain.Entities.HistoryRecord(result));
                await unitOfWork.SaveChangesAsync();
                return Ok(result);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Quantity days must be between 1 and 16");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("City can't be whitespaces or empty");
            }
            catch (ArgumentException)
            {
                return BadRequest("City can't be null");
            }
            catch (AggregateException)
            {
                return BadRequest("Error occured");
            }
            catch (HttpRequestException)
            {
                return BadRequest("Bad city name");
            }
            
        }


        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            weatherService.Dispose();
            base.Dispose(disposing);
        }
    }
}
