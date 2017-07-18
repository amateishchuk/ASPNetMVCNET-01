using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Controllers.Api;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.IntegrationTests.Api
{
    [TestFixture]
    public class IntegrationHistoryControllerApiTests
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly HistoryController controller;

        public IntegrationHistoryControllerApiTests()
        {
            unitOfWork = new UnitOfWork("TestDb");
            controller = new HistoryController(unitOfWork);
        }
        [Test]
        public void IntegrationGetHistoryApi_When_HistoryRepoContain1Record_Then_ReturnThatRecord()
        {
            string city = "Lviv";
            unitOfWork.History.Insert(new Domain.Entities.HistoryRecord
            {
                City = city,
                DateTime = DateTime.Now,
                DayData = new Domain.OwmService.DayData
                {
                    Clouds = 10
                }
            });
            unitOfWork.SaveChanges();
            var record = unitOfWork.History.Get(r => r.City == city);

            var result = controller.GetHistory() as OkNegotiatedContentResult<IEnumerable<HistoryRecord>>;
            unitOfWork.History.Delete(record);
            unitOfWork.SaveChanges();

            Assert.IsTrue(result.Content.Contains(record));            
        }

        
    }
}
