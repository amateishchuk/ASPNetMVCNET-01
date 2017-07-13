using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Concrete;
using System.Configuration;

namespace WeatherApp.Tests.IntegrationTests
{
    [TestFixture]
    public class WeatherServiceTests
    {
        string apiUrl = ConfigurationManager.AppSettings["ApiUriOwm"];
        string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
        WeatherServiceOwm weatherService;

        public WeatherServiceTests()
        {
            weatherService = new WeatherServiceOwm(apiKey, apiUrl);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(17)]
        public void IntegrationGetWeatherInfo_When_QtyDaysIsOutOfRange_Then_ThrowArgumentOutOfRangeException(int qtyDays)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => weatherService.GetWeatherInfo("DefaultCity", qtyDays));
        }

        [Test]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase("")]
        public void IntegrationGetWeatherInfo_When_BadString_Then_ThrowArgumentException(string city)
        {
            Assert.Throws<ArgumentNullException>(() => weatherService.GetWeatherInfo(city, 1));
        }
        [Test]
        [TestCase("Kiev", 10)]
        public void IntegrationGetWeatherInfo_When_ValidData_Then_ValidResult(string city, int qtyDays)
        {
            var result = weatherService.GetWeatherInfo(city, qtyDays);

            Assert.AreEqual(city, result.City.Name);
            Assert.AreEqual(qtyDays, result.Cnt);
        }
    }
}
