using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult GetExtendedHistory()
        {
            var fullHistory = unitOfWork.History.GetAll();
            return View(fullHistory);
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}