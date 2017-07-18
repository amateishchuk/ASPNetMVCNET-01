using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using WeatherApp.Controllers.Api;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.OwmService;

namespace WeatherApp.Tests.UnitTests.Api
{
    [TestFixture]
    public class UnitWeatherControllerApiTests
    {
        private readonly Mock<IWeatherService> mockWeatherService;
        private readonly Mock<IRepository<HistoryRecord>> mockHistoryRepo;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly WeatherController controller;
        private List<HistoryRecord> history;        

        public UnitWeatherControllerApiTests()
        {
            history = new List<HistoryRecord>();
            mockWeatherService = new Mock<IWeatherService>();
            mockHistoryRepo = new Mock<IRepository<HistoryRecord>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            controller = new WeatherController(mockUnitOfWork.Object, mockWeatherService.Object);
        }

        [SetUp]
        public void TestSetup()
        {
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange(1, 16, Range.Inclusive)))
                .Returns(new WeatherOwm() { City = new City { Name = "Kiev" }, List = new List<Domain.OwmService.DayData> {
                    new Domain.OwmService.DayData() } });
            mockWeatherService.Setup(w => w.GetWeather(It.Is<string>(s => s == null), It.IsInRange(1, 16, Range.Inclusive)))
                .Throws<ArgumentNullException>();
            mockWeatherService.Setup(w => w.GetWeather(It.Is<string>(s => s == ""), It.IsInRange(1, 16, Range.Inclusive)))
                .Throws<ArgumentException>();
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[0-9]"), It.IsInRange(1, 16, Range.Inclusive)))
                .Throws<ArgumentException>();
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.Is<int>(qtyDays => qtyDays < 1 ||qtyDays > 16)))
                .Throws<ArgumentOutOfRangeException>();


            mockHistoryRepo.Setup(r => r.Insert(It.IsAny<HistoryRecord>())).Callback((HistoryRecord r) => history.Add(r));
            mockUnitOfWork.Setup(u => u.History).Returns(mockHistoryRepo.Object);
            mockUnitOfWork.Setup(u => u.History.Insert(It.IsAny<HistoryRecord>()))
                .Callback((HistoryRecord r) => mockHistoryRepo.Object.Insert(r));
        }
        [Test]
        [TestCase("Kiev", 10)]
        public void UnitApiGetWeather_When_ParametersValid_Then_ReturnNewWeatherOwm(string name, int qtyDays)
        {
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange(1, 16, Range.Inclusive)))
                .Returns(new WeatherOwm()
                {
                    City = new City { Name = name },
                    Cnt = qtyDays,
                    List = new List<Domain.OwmService.DayData> {
                        new Domain.OwmService.DayData(),
                        new Domain.OwmService.DayData()
                    }
                });
            WeatherController controller = new WeatherController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.GetWeather(name, qtyDays) as OkNegotiatedContentResult<WeatherOwm>;

            Assert.AreEqual(name, result.Content.City.Name);
            Assert.AreEqual(qtyDays, result.Content.Cnt);
        }
                
        [Test]
        [TestCase("Kiev", 20)]
        [TestCase(null, 10)]
        [TestCase("", 10)]
        [TestCase("123Kiev", 10)]
        public void UnitApiGetWeather_When_QtyDaysIsAboveThan16_Then_ThrowsArgumentOutOfRangeException(string name, int qtyDays)
        {
            var result = controller.GetWeather(name, qtyDays) as BadRequestErrorMessageResult;

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), result);
        }
    }
}
