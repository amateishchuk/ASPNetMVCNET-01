using System.Net;
using System.Net.Http;
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

        public HttpResponseMessage GetAllHistory()
        {
            return Request.CreateResponse(HttpStatusCode.OK, unitOfWork.History.GetAll());
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
