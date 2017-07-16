using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using System.Configuration;
using WeatherApp.Tests.Fake;
using WeatherApp.Controllers;
using System.Web.Mvc;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class WeatherControllerTests
    {
        IUnitOfWork unitOfWork;
        IWeatherService weatherService;

        public WeatherControllerTests()
        {
            unitOfWork = new FakeUnitOfWork();
            weatherService = new FakeWeatherService();
        }

        [Test]
        [TestCase("", 1)]
        [TestCase("  ", 1)]
        [TestCase(null, 1)]
        [TestCase("Kiev", -3)]
        [TestCase("Kharkiv", 19)]
        public void UnitShowWeather_When_ParametersNotValid_Then_HttpNotFoundResult(string city, int qty)
        {
            Controllers.WeatherController controller = new Controllers.WeatherController(weatherService, unitOfWork);

            var result = controller.ShowWeather(city, qty);

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase("Kiev", 10)]
        public void UnitShowWeather_When_ParametersValid_Then_ReturnWeatherOWN(string city, int qty)
        {
            Controllers.WeatherController controller = new Controllers.WeatherController(weatherService, unitOfWork);

            var result = controller.ShowWeather(city, qty);

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
    }
}
