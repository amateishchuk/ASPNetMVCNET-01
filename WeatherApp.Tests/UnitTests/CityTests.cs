using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Entities;
using WeatherApp.Tests.Fake;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class CityTests
    {
        private readonly FakeUnitOfWork _fakeUnitOfWork;
        private readonly FakeRepository<City> _fakeCityRepository;
        private readonly FakeRepository<HistoryRecord> _fakeHistoryRecordRepository;

        public CityTests()
        {
            _fakeUnitOfWork = new FakeUnitOfWork();
            _fakeCityRepository = new FakeRepository<City>();
            _fakeHistoryRecordRepository = new FakeRepository<HistoryRecord>();
        }

        [SetUp]
        public void TestSetup()
        {
            var kiev = new City { Id = 1, Name = "Kiev" };
            var kharkiv = new City { Id = 2, Name = "Kharkiv" };
            
            _fakeCityRepository.Data.AddRange(new[] { kiev, kharkiv });

            _fakeUnitOfWork.SetRepository(_fakeCityRepository);
        }
        [TearDown]
        public void TestTearDown()
        {
            _fakeCityRepository.Data.Clear();
            _fakeHistoryRecordRepository.Data.Clear();
        }

        [Test]
        [TestCase("Kiev")]
        public void UnitAddCity_When_CityExistsInList_Then_CityCountLeftConstant(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(_fakeUnitOfWork);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(2, _fakeUnitOfWork.Repository<City>().GetAll().Count());
        }
        [Test]
        [TestCase("Vinnitsa")]
        public void UnitAddCity_When_CityDoesntExistInList_Then_CityCountUpOne(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(_fakeUnitOfWork);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(3, _fakeUnitOfWork.Repository<City>().GetAll().Count());
        }
        [Test]
        [TestCase(0)]
        public void UnitEditCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(_fakeUnitOfWork);

            // Act
            var result = cityController.Edit(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase(0)]
        [TestCase(5)]
        public void UnitDeleteCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(_fakeUnitOfWork);

            // Act
            var result = cityController.Delete(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase(1)]
        public void UnitDeleteCity_When_CityWithCorrectId_CityCountDownOne(int id)
        {
            // Arrange
            CityController cityController = new CityController(_fakeUnitOfWork);

            // Act
            var result = cityController.DeleteConfirm(id);

            // Assert
            Assert.AreEqual(1, _fakeUnitOfWork.Repository<City>().GetAll().Count());
        }
    }
}
