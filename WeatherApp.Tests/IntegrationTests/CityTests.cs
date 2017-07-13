using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.IntegrationTests
{
    [TestFixture]
    public class CityTests
    {
        private readonly UnitOfWork unitOfWork;

        public CityTests()
        {
            unitOfWork = new UnitOfWork("TestDb");
        }

        [SetUp]
        public void TestSetup()
        {
            var kiev = new City { Id = 1, Name = "Kiev" };
            var kharkiv = new City { Id = 2, Name = "Kharkiv" };

            unitOfWork.Repository<City>().Insert(kiev);
            unitOfWork.Repository<City>().Insert(kharkiv);
            unitOfWork.SaveChanges();
        }
        [TearDown]
        public void TestTearDown()
        {
            foreach (var city in unitOfWork.Repository<City>().GetAll())
                unitOfWork.Repository<City>().Delete(city);
            unitOfWork.SaveChanges();
        }

        [Test]
        [TestCase("Kiev")]
        public void IntegrationAddCity_When_CityExistsInList_Then_CityCountLeftConstant(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(2, unitOfWork.Repository<City>().GetAll().Count());
        }
        [Test]
        [TestCase("Vinnitsa")]
        public void IntegrationAddCity_When_CityDoesntExistInList_Then_CityCountUpOne(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(3, unitOfWork.Repository<City>().GetAll().Count());
        }
        [Test]
        [TestCase(0)]
        public void IntegrationEditCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork);

            // Act
            var result = cityController.Edit(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        [Test]
        [TestCase(0)]
        [TestCase(5)]
        public void IntegrationDeleteCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork);

            // Act
            var result = cityController.Delete(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }


        //// SET_IDENTITY_INSERT ON;
        //[Test]
        //[TestCase(1)]
        //public void IntegrationDeleteCity_When_CityWithCorrectId_CityCountDownOne(int id)
        //{
        //    // Arrange
        //    CityController cityController = new CityController(unitOfWork);

        //    // Act
        //    var result = cityController.DeleteConfirm(id);

        //    // Assert
        //    Assert.AreEqual(1, unitOfWork.Repository<City>().GetAll().Count());
        //}
    }
}
