using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;

namespace WeatherApp.Controllers
{
    public class WeatherHistoryController : Controller
    {
        IUnitOfWork db;
        public WeatherHistoryController(IUnitOfWork dbService)
        {
            db = dbService;
        }

        public ActionResult GetHistory()
        {
            return PartialView(db.HistoryRecords.GetAll().Take(5));
        }
        public ActionResult GetExtendedHistory()
        {
            var result = db.HistoryRecords.GetAll();
            return View(result);
        }
    }
}