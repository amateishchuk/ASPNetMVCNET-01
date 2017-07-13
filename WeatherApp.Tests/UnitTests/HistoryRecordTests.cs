using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Controllers;
using WeatherApp.Domain.Entities;
using WeatherApp.Tests.Fake;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class HistoryRecordTests
    {
        private readonly FakeUnitOfWork _fakeUnitOfWork;
        private readonly FakeRepository<HistoryRecord> _fakeHistoryRecordRepository;
        private readonly FakeWeatherService fakeWeatherService;

        public HistoryRecordTests()
        {
            _fakeUnitOfWork = new FakeUnitOfWork();
            _fakeHistoryRecordRepository = new FakeRepository<HistoryRecord>();
            fakeWeatherService = new FakeWeatherService();
        }
        [SetUp]
        public void TestSetup()
        {
            var historyRec01 = new HistoryRecord { City = "City01" };
            var historyRec02 = new HistoryRecord { City = "City02" };
            var historyRec03 = new HistoryRecord { City = "City03" };
            var historyRec04 = new HistoryRecord { City = "City04" };
            var historyRec05 = new HistoryRecord { City = "City05" };

            var historyRec06 = new HistoryRecord { City = "City06" };
            var historyRec07 = new HistoryRecord { City = "City07" };
            var historyRec08 = new HistoryRecord { City = "City08" };
            var historyRec09 = new HistoryRecord { City = "City09" };
            var historyRec10 = new HistoryRecord { City = "City10" };

            var historyRec11 = new HistoryRecord { City = "City11" };
            var historyRec12 = new HistoryRecord { City = "City12" };
            var historyRec13 = new HistoryRecord { City = "City13" };

            
            _fakeHistoryRecordRepository.Data.AddRange(new[]
            {
                historyRec01, historyRec02, historyRec03, historyRec04, historyRec05,
                historyRec06, historyRec07, historyRec08, historyRec09, historyRec10,
                historyRec11, historyRec12, historyRec13
            });
            _fakeUnitOfWork.SetRepository(_fakeHistoryRecordRepository);
        }
        [TearDown]
        public void TestTearDown()
        {
            _fakeHistoryRecordRepository.Data.Clear();
        }
        [Test]
        public void UnitAddRecord_When_RecordsCount15_Then_DeleteFirstAndAddCurrent()
        {
            // Arrange
            WeatherController weatherController = new WeatherController(fakeWeatherService, _fakeUnitOfWork);

            // Act
            weatherController.ShowWeather("City14", 10);
            weatherController.ShowWeather("City15", 10);
            weatherController.ShowWeather("City16", 10);

            // Assert
            Assert.AreEqual("City02", _fakeUnitOfWork.Repository<HistoryRecord>().First.City);
            Assert.AreEqual("City16", _fakeUnitOfWork.Repository<HistoryRecord>().GetAll().Last().City);
            Assert.AreEqual(15, _fakeUnitOfWork.Repository<HistoryRecord>().Count);
        }
        [Test]
        public void UnitAddRecord_When_RecordsCount13_Then_AddOneWithoutDeleting()
        {
            // Arrange
            WeatherController weatherController = new WeatherController(fakeWeatherService, _fakeUnitOfWork);

            // Act
            weatherController.ShowWeather("City14", 10);

            // Assert
            Assert.AreEqual("City01", _fakeUnitOfWork.Repository<HistoryRecord>().First.City);
            Assert.AreEqual("City14", _fakeUnitOfWork.Repository<HistoryRecord>().GetAll().Last().City);
            Assert.AreEqual(14, _fakeUnitOfWork.Repository<HistoryRecord>().GetAll().Count());
        }
    }
}
