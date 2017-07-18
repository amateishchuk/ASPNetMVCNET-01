using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.OwmService;

namespace WeatherApp.Tests.IntegrationTests
{
    [TestFixture]
    public class IntegrationWeatherControllerTests
    {
        IUnitOfWork unitOfWork;
        IWeatherService weatherService;
        WeatherController controller;

        public IntegrationWeatherControllerTests()
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
            string apiUri = ConfigurationManager.AppSettings["ApiUriOwm"];
            unitOfWork = new UnitOfWork("TestDb");
            weatherService = new WeatherServiceOwm(apiKey, apiUri);
            controller = new WeatherController(weatherService, unitOfWork);
        }

        [Test]
        [TestCase("", 1)]
        [TestCase("  ", 1)]
        [TestCase(null, 1)]
        [TestCase("Kiev", -3)]
        [TestCase("Kharkiv", 19)]
        public void IntegrationShowWeather_When_ParametersNotValid_Then_HttpNotFoundResult(string city, int qty)
        {            
            var result = controller.GetWeather(city, qty);

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase("Kiev", 10)]
        public void IntegrationShowWeather_When_ParametersValid_Then_ReturnWeatherOWN(string city, int qty)
        {            
            var result = controller.GetWeather(city, qty) as ViewResult;
            var model = result.Model as WeatherOwm;
            var record = unitOfWork.History.GetAll().First();
            unitOfWork.History.Delete(record);
            unitOfWork.SaveChanges();

            Assert.AreEqual(city, model.City.Name);            
        }
    }
}
