using NUnit.Framework;
using System;
using WeatherApp.Tests.Fake;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class WeatherServiceTests
    {
        FakeWeatherService fakeWeatherService;

        public WeatherServiceTests()
        {
            fakeWeatherService = new FakeWeatherService();
        }

        [Test]        
        [TestCase(-1)]
        [TestCase(17)]
        public void UnitGetWeatherInfo_When_QtyDaysIsOutOfRange_Then_ThrowArgumentOutOfRangeException(int qtyDays)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => fakeWeatherService.GetWeatherInfo("DefaultCity", qtyDays)); 
        }

        [Test]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase("")]
        public void UnitGetWeatherInfo_When_BadString_Then_ThrowArgumentException(string city)
        {
            Assert.Throws<ArgumentNullException>(() => fakeWeatherService.GetWeatherInfo(city, 1));
        }
        [Test]
        [TestCase("Kiev", 10)]        
        public void UnitGetWeatherInfo_When_BadString_Then_ThrowArgumentException(string city, int qtyDays)
        {
            var result = fakeWeatherService.GetWeatherInfo(city, qtyDays);

            Assert.AreEqual(city, result.City.Name);
            Assert.AreEqual(qtyDays, result.Cnt);
        }
    }
}
