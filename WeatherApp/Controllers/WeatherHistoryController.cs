using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Controllers
{
    public class WeatherHistoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public WeatherHistoryController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public ActionResult GetHistory()
        {
            var smallList = unitOfWork.History.GetAll().Take(5);
            return PartialView(smallList);
        }
        public async Task<ActionResult> GetExtendedHistory()
        {
            var fullHistory = await unitOfWork.History.GetAllAsync();
            return View(fullHistory);
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}