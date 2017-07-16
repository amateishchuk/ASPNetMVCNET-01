using NUnit.Framework;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.IntegrationTests
{
    [TestFixture]
    public class CityControllerTests
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWeatherService weatherService;

        public CityControllerTests()
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
            string apiUri = ConfigurationManager.AppSettings["ApiUriOwm"];
            unitOfWork = new UnitOfWork("TestDb");
            weatherService = new WeatherServiceOwm(apiKey, apiUri);
        }

        [SetUp]
        public void TestSetup()
        {
            var kiev = new City { Id = 1, Name = "Kiev" };
            var kharkiv = new City { Id = 2, Name = "Kharkiv" };

            unitOfWork.Cities.Insert(kiev);
            unitOfWork.Cities.Insert(kharkiv);
        }
        [TearDown]
        public void TestTearDown()
        {
            foreach (var city in unitOfWork.Cities.GetAll())
                unitOfWork.Cities.Delete(city);
        }                    
        [TestCase("Vinnitsa")]
        public void IntegrationAddCity_When_CityDoesntExistInList_Then_CityCountUpOne(string cityName)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork, weatherService);

            // Act
            cityController.Add(cityName);

            // Assert
            Assert.AreEqual(3, unitOfWork.Cities.GetAll().Count());
        }
        [Test]
        [TestCase(0)]
        public void IntegrationEditCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork, weatherService);

            // Act
            var result = cityController.Edit(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }


        [Test]
        [TestCase(0)]
        public void IntegrationDeleteCity_When_CityWithIncorrectId_Then_ReturnErrorPage(int id)
        {
            // Arrange
            CityController cityController = new CityController(unitOfWork, weatherService);

            // Act
            var result = cityController.Delete(id);

            // Assert
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }        
    }
}
