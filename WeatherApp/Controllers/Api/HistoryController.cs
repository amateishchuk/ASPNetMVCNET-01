using System.Web.Http;
using WeatherApp.Domain.Abstract;

namespace WeatherApp.Controllers.Api
{
    public class HistoryController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public HistoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IHttpActionResult GetHistory()
        {
            var result = unitOfWork.History.GetAll();

            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
