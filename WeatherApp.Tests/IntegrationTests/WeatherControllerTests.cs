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

namespace WeatherApp.Tests.IntegrationTests
{
    [TestFixture]
    public class WeatherControllerTests
    {
        IUnitOfWork unitOfWork;
        IWeatherService weatherService;

        public WeatherControllerTests()
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
            string apiUri = ConfigurationManager.AppSettings["ApiUriOwm"];
            unitOfWork = new UnitOfWork("TestDb");
            weatherService = new WeatherServiceOwm(apiKey, apiUri);
        }

        [Test]
        [TestCase("", 1)]
        [TestCase("  ", 1)]
        [TestCase(null, 1)]
        [TestCase("Kiev", -3)]
        [TestCase("Kharkiv", 19)]
        public void IntegrationShowWeather_When_ParametersNotValid_Then_HttpNotFoundResult(string city, int qty)
        {
            WeatherController controller = new WeatherController(weatherService, unitOfWork);

            var result = controller.ShowWeather(city, qty);

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase("Kiev", 10)]
        public void IntegrationShowWeather_When_ParametersValid_Then_ReturnWeatherOWN(string city, int qty)
        {
            WeatherController controller = new WeatherController(weatherService, unitOfWork);
            

            var result = controller.ShowWeather(city, qty);

            
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
        [Test]
        [TestCase("Kharkiv", 5)]
        public void IntegrationShowWeather_When_ParametersValid_Then_HistoryRecordInRepoOneUp(string city, int qty)
        {
            WeatherController controller = new WeatherController(weatherService, unitOfWork);

            var count = unitOfWork.History.GetAll().Count();
            var result = controller.ShowWeather(city, qty);


            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(count + 1, unitOfWork.History.GetAll().Count());
            Assert.AreEqual(city, unitOfWork.History.GetAll().First().City);            
        }
    }
}
