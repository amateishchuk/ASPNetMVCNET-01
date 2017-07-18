using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Controllers.Api;
using System.Web.Http.Results;

namespace WeatherApp.Tests.UnitTests.Api
{
    [TestFixture]
    public class UnitHistoryContollerApiTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IRepository<HistoryRecord>> mockHistoryRepo;
        private List<HistoryRecord> history;

        public UnitHistoryContollerApiTests()
        {
            mockHistoryRepo = new Mock<IRepository<HistoryRecord>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();           
        }
        [SetUp]
        public void TestSetup()
        {
            history = new List<HistoryRecord>();
            mockHistoryRepo.Setup(r => r.GetAll()).Returns(history);
            mockUnitOfWork.Setup(u => u.History).Returns(mockHistoryRepo.Object);
            mockUnitOfWork.Setup(u => u.History.GetAll()).Returns(mockHistoryRepo.Object.GetAll());
        }
        [Test]
        public void UnitApiGetHistory_When_HistoryContain5Records_Then_ResultContentCountEqual5()
        {            
            history.Add(
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
                });
            HistoryController controller = new HistoryController(mockUnitOfWork.Object);

            var result = controller.GetHistory() as OkNegotiatedContentResult<IEnumerable<HistoryRecord>>;

            Assert.That(result.Content.ToList().Count == 1);
        }
    }
}
