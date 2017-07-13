using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Controllers;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.IntegrationTests
{
    [TestFixture]
    public class HistoryRecordTests
    {
        private readonly UnitOfWork unitOfWork;
        private readonly WeatherServiceOwm weatherService;

        public HistoryRecordTests()
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
            string apiUrl = ConfigurationManager.AppSettings["ApiUriOwm"];
            unitOfWork = new UnitOfWork("TestDb");
            weatherService = new WeatherServiceOwm(apiKey, apiUrl);
        }
        [SetUp]
        public void TestSetup()
        {
            var historyRec01 = new HistoryRecord { Id = 1, City = "City01" };
            var historyRec02 = new HistoryRecord { Id = 2, City = "City02" };
            var historyRec03 = new HistoryRecord { Id = 3, City = "City03" };
            var historyRec04 = new HistoryRecord { City = "City04", Id = 4 };
            var historyRec05 = new HistoryRecord { City = "City05", Id = 5 };

            var historyRec06 = new HistoryRecord { City = "City06", Id = 6 };
            var historyRec07 = new HistoryRecord { City = "City07", Id = 7 };
            var historyRec08 = new HistoryRecord { City = "City08", Id = 8 };
            var historyRec09 = new HistoryRecord { City = "City09", Id = 9 };
            var historyRec10 = new HistoryRecord { City = "City10", Id = 10 };

            var historyRec11 = new HistoryRecord { City = "City11", Id = 11 };
            var historyRec12 = new HistoryRecord { City = "City12", Id = 12 };
            var historyRec13 = new HistoryRecord { City = "City13", Id = 13 };


            unitOfWork.Repository<HistoryRecord>().Insert(historyRec01);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec02);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec03);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec04);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec05);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec06);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec07);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec08);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec09);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec10);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec11);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec12);
            unitOfWork.Repository<HistoryRecord>().Insert(historyRec13);

            unitOfWork.SaveChanges();
        }
        [TearDown]
        public void TestTearDown()
        {
            foreach(var historyRec in unitOfWork.Repository<HistoryRecord>().GetAll())
                unitOfWork.Repository<HistoryRecord>().Delete(historyRec);
            unitOfWork.SaveChanges();
        }
        //[Test]
        //public void IntegrationAddRecord_When_RecordsCount15_Then_DeleteFirstAndAddCurrent()
        //{
        //    // Arrange
        //    WeatherController weatherController = new WeatherController(weatherService, unitOfWork);

        //    // Act
        //    weatherController.ShowWeather("City14", 10);
        //    weatherController.ShowWeather("City15", 10);
        //    weatherController.ShowWeather("City16", 10);

        //    // Assert
        //    Assert.AreEqual("City02", unitOfWork.Repository<HistoryRecord>().First.City);
        //    Assert.AreEqual("City16", unitOfWork.Repository<HistoryRecord>().GetAll().Last().City);
        //    Assert.AreEqual(15, unitOfWork.Repository<HistoryRecord>().Count);
        //}
        //[Test]
        //public void IntegrationAddRecord_When_RecordsCount13_Then_AddOneWithoutDeleting()
        //{
        //    // Arrange
        //    WeatherController weatherController = new WeatherController(weatherService, unitOfWork);

        //    // Act
        //    weatherController.ShowWeather("City14", 10);

        //    // Assert
        //    Assert.AreEqual("City01", unitOfWork.Repository<HistoryRecord>().First.City);
        //    Assert.AreEqual("City14", unitOfWork.Repository<HistoryRecord>().GetAll().Last().City);
        //    Assert.AreEqual(14, unitOfWork.Repository<HistoryRecord>().GetAll().Count());
        //}
    }
}
