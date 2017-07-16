using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Tests.Fake;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class CityControllerTests
    {
        private FakeUnitOfWork fakeUnitOfWork;
        private FakeWeatherService fakeWeatherService;

        public CityControllerTests()
        {
            fakeUnitOfWork = new FakeUnitOfWork();
            fakeWeatherService = new FakeWeatherService();
        }

        [SetUp]
        public void TestSetup()
        {
            var kiev = new City { Id = 1, Name = "Kiev" };
            var kharkiv = new City { Id = 2, Name = "Kharkiv" };

            fakeUnitOfWork.Cities.Insert(kiev);
            fakeUnitOfWork.Cities.Insert(kharkiv);           
        }
        [TearDown]
        public void TestTearDown()
        {
            foreach (var city in fakeUnitOfWork.Cities.GetAll())
                fakeUnitOfWork.Cities.Delete(city.Id);
        }

        [Test]
        public void UnitGetFavourites_When_CollectionHas2Records_Then_ModelMustContains2Records()
        {
            CityController controller = new CityController(fakeUnitOfWork, fakeWeatherService);

            //var result = controller.GetFavorites() as ViewResult;


            // Действие
            List<City> result = 
                ((IEnumerable<City>)controller.GetFavorites().ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual("Kharkiv", result[0].Name);
            Assert.AreEqual("Kiev", result[1].Name);            
        }

        [Test]
        [TestCase("Kiev")]
        public void UnitAddCity_When_CityExistsInList_Then_CityCountLeftConstant(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(fakeUnitOfWork, fakeWeatherService);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(2, fakeUnitOfWork.Cities.GetAll().Count());
        }
        [Test]
        [TestCase("Vinnitsa")]
        public void UnitAddCity_When_CityDoesntExistInList_Then_CityCountUpOne(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(fakeUnitOfWork, fakeWeatherService);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(3, fakeUnitOfWork.Cities.GetAll().Count());
        }
        [Test]
        [TestCase(0)]
        public void UnitEditCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(fakeUnitOfWork, fakeWeatherService);

            // Act
            var result = cityController.Edit(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }


        [Test]
        [TestCase(0)]
        public void UnitDeleteCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(fakeUnitOfWork, fakeWeatherService);

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
            CityController cityController = new CityController(fakeUnitOfWork, fakeWeatherService);

            // Act
            var result = cityController.DeleteConfirm(id);

            // Assert
            Assert.AreEqual(1, fakeUnitOfWork.Cities.GetAll().Count());
        }
    }
}
