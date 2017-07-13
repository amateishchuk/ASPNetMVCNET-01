﻿using System;
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
        IUnitOfWork uow;
        public WeatherHistoryController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public ActionResult GetHistory()
        {
            var smallList = uow.Repository<HistoryRecord>().GetAll().OrderByDescending(r => r.Id).Take(5);

            return PartialView(smallList);
        }
        public ActionResult GetExtendedHistory()
        {
            var fullHistory = uow.Repository<HistoryRecord>().GetAll().OrderByDescending(r => r.Id);
            return View(fullHistory);
        }
    }
}