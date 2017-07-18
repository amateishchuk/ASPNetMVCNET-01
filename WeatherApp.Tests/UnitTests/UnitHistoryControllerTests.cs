using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class UnitHistoryControllerTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IRepository<HistoryRecord>> mockHistoryRepo;
        private readonly WeatherHistoryController controller;
        private List<HistoryRecord> history;

        public UnitHistoryControllerTests()
        {
            mockHistoryRepo = new Mock<IRepository<HistoryRecord>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            controller = new WeatherHistoryController(mockUnitOfWork.Object);
        }
        [SetUp]
        public void TestSetup()
        {
            history = new List<HistoryRecord>();
            history.AddRange(new[] {
                new HistoryRecord
                {
                    Id = 1,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
                new HistoryRecord
                {
                    Id = 2,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
                new HistoryRecord
                {
                    Id = 3,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
                new HistoryRecord
                {
                    Id = 4,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
                new HistoryRecord
                {
                    Id = 5,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
                new HistoryRecord
                {
                    Id = 6,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
                new HistoryRecord
                {
                    Id = 7,
                    City = "Kiev",
                    DateTime = DateTime.Now,
                    DayData = new Domain.OwmService.DayData
                    {
                        Id = 1,
                        Clouds = 50,
                        Deg = 100,
                        Dt = 1000,
                        Humidity = 10,
                        Pressure = 10,
                        Speed = 10,
                        Rain = 10,
                        Temp = null,
                        Weather = null
                    }
                },
            });
            mockHistoryRepo.Setup(r => r.GetAll()).Returns(history);
            mockUnitOfWork.Setup(u => u.History).Returns(mockHistoryRepo.Object);
            mockUnitOfWork.Setup(u => u.History.GetAll()).Returns(mockHistoryRepo.Object.GetAll());
        }

        [Test]
        public void UnitApiGeExtendedtHistory_When_HistoryContain10Records_Then_ResultContentCountEqual10()
        {
            var result = controller.GetExtendedHistory() as ViewResult;
            var model = result.Model as IEnumerable<HistoryRecord>;

            Assert.That(model.ToList().Count == history.Count);
        }

        [Test]
        public void UnitApiGetHistory_When_HistoryContain13Records_Then_ResultContentCountEqual5()
        {
            var result = controller.GetHistory() as PartialViewResult;
            var model = result.Model as IEnumerable<HistoryRecord>;

            Assert.That(model.ToList().Count == 5);
        }
    }
}
