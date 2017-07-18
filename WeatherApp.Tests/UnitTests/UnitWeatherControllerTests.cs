using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.OwmService;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class UnitWeatherControllerTests
    {
        private readonly Mock<IWeatherService> mockWeatherService;
        private readonly Mock<IRepository<HistoryRecord>> mockHistoryRepo;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly WeatherController controller;
        private List<HistoryRecord> history;

        public UnitWeatherControllerTests()
        {
            history = new List<HistoryRecord>();
            mockWeatherService = new Mock<IWeatherService>();
            mockHistoryRepo = new Mock<IRepository<HistoryRecord>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            controller = new WeatherController(mockWeatherService.Object, mockUnitOfWork.Object);
        }
        [SetUp]
        public void TestSetup()
        {
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange(1, 16, Range.Inclusive)))
                .Returns(new WeatherOwm()
                {
                    City = new City { Name = "Kiev" },
                    List = new List<Domain.OwmService.DayData> {
                    new Domain.OwmService.DayData() }
                });
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm());
            mockWeatherService.Setup(w => w.GetWeather(It.Is<string>(c => c == null), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Throws<ArgumentNullException>();
            mockWeatherService.Setup(w => w.GetWeather(It.Is<string>(c => string.IsNullOrEmpty(c) || string.IsNullOrWhiteSpace(c)), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Throws<ArgumentException>();
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.Is<int>(qty => qty < 1 || qty > 16)))
                .Throws<ArgumentOutOfRangeException>();


            mockHistoryRepo.Setup(r => r.Insert(It.IsAny<HistoryRecord>())).Callback((HistoryRecord r) => history.Add(r));
            mockUnitOfWork.Setup(u => u.History).Returns(mockHistoryRepo.Object);
            mockUnitOfWork.Setup(u => u.History.Insert(It.IsAny<HistoryRecord>()))
                .Callback((HistoryRecord r) => mockHistoryRepo.Object.Insert(r));
        }


        [Test]
        [TestCase("", 1)]
        [TestCase("  ", 1)]
        [TestCase(null, 1)]
        [TestCase("Kiev", -3)]
        [TestCase("Kharkiv", 19)]
        public void UnitGetWeather_When_ParametersNotValid_Then_HttpNotFoundResult(string city, int qty)
        {
            var result = controller.GetWeather(city, qty);

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase("Kiev", 10)]
        public void UnitShowWeather_When_ParametersValid_Then_ReturnWeatherOWN(string city, int qty)
        {
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange(1, 16, Range.Inclusive)))
                .Returns(new WeatherOwm()
                {
                    City = new City { Name = city },
                    Cnt = qty,
                    List = new List<Domain.OwmService.DayData> {
                        new Domain.OwmService.DayData(),
                        new Domain.OwmService.DayData()
                    }
                });

            var result = controller.GetWeather(city, qty) as ViewResult;
            var model = result.Model as WeatherOwm;

            Assert.AreEqual(city, model.City.Name);
            Assert.AreEqual(qty, model.Cnt);
        }
    }
}
