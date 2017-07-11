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
        IRepository repository;
        public WeatherHistoryController(IRepository repo)
        {
            repository = repo;
        }

        public ActionResult GetHistory()
        {
            return PartialView(repository.LastRequests.Take(5));
        }
        public ActionResult GetExtendedHistory()
        {
            var result = repository.LastRequests;
            return View(result);
        }
    }
}