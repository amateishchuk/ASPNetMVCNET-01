using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Controllers.Api;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using System.Web.Http.Results;
using WeatherApp.OwmService;

namespace WeatherApp.Tests.IntegrationTests.Api
{
    [TestFixture]
    public class IntegrationWeatherControllerApiTests
    {
        private readonly IUnitOfWork unitOfwork;
        private readonly IWeatherService weatherService;
        private readonly WeatherController controller;

        public IntegrationWeatherControllerApiTests()
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
            string apiUri = ConfigurationManager.AppSettings["ApiUriOwm"];

            unitOfwork = new UnitOfWork("TestDb");
            weatherService = new WeatherServiceOwm(apiKey, apiUri);
            controller = new WeatherController(unitOfwork, weatherService);
        }
        
        [Test]
        public void IntegrationGetWeather_When_DataValid_Then_ReturnOkWithWeather()
        {
            string city = "Lviv";
            int qtyDays = 1;

            var result = controller.GetWeather(city, qtyDays) as OkNegotiatedContentResult<WeatherOwm>;

            var record = unitOfwork.History.Get(r => r.City == city);
            unitOfwork.History.Delete(record);
            unitOfwork.SaveChanges();

            Assert.That(result.Content.City.Name == city);
            Assert.That(result.Content.Cnt == qtyDays);
            Assert.That(result.Content.List.Count == qtyDays);
        }

        [Test]
        [TestCase(null, 1)]
        [TestCase("", 1)]
        [TestCase("     ", 1)]
        [TestCase("InvalidNameCity", 1)]
        [TestCase("Kiev", 35)]
        public void IntegrationGetWeather_When_DataInValid_Then_ReturnBadRequest(string city, int qtyDays)
        {
            var result = controller.GetWeather(city, qtyDays) as BadRequestErrorMessageResult;

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), result);
        }

    }
}
