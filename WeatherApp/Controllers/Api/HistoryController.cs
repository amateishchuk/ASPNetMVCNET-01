using System.Threading.Tasks;
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

        public async Task<IHttpActionResult> GetHistory()
        {
            var result = await unitOfWork.History.GetAllAsync();

            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
