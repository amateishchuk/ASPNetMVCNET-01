using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using WeatherApp.Controllers.Api;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.UnitTests.Api
{
    [TestFixture]
    public class UnitCityControllerApiTests
    {
        private readonly Mock<IRepository<City>> mockCityRepo;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IWeatherService> mockWeatherService;
        private readonly CityController controller;
        private List<City> cities;

        public UnitCityControllerApiTests()
        {
            mockCityRepo = new Mock<IRepository<City>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockWeatherService = new Mock<IWeatherService>();
            controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);
        }

        [SetUp]
        public void TestSetUp()
        {
            mockCityRepo.Setup(m => m.GetAll()).Returns(cities);            
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>()))
               .Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockCityRepo.Setup(r => r.Insert(It.IsAny<City>())).Callback((City c) =>
            {
                var city = cities.FirstOrDefault(ct => ct.Name == c.Name);
                if (city == null)
                    cities.Add(c);
            });
            mockCityRepo.Setup(m => m.Delete(It.IsAny<City>())).Callback((City city) =>
            {
                cities.Remove(city);
            });
            mockCityRepo.Setup(r => r.Update(It.IsAny<City>())).Callback((City city) =>
            {                
                var oldCity = cities.FirstOrDefault(ci => ci.Id == city.Id);
                if (oldCity != null)
                    cities.Remove(oldCity);
                cities.Add(city);
            });

            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockUnitOfWork.Setup(u => u.Cities.GetAll()).Returns(mockCityRepo.Object.GetAll());
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Insert(It.IsAny<City>())).Callback((City c) => mockCityRepo.Object.Insert(c));
            mockUnitOfWork.Setup(u => u.Cities.Delete(It.IsAny<City>())).Callback((City city) => mockCityRepo.Object.Delete(city));
            mockUnitOfWork.Setup(u => u.Cities.Update(It.IsAny<City>())).Callback((City c) => mockCityRepo.Object.Update(c));


            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm());
        }

        [Test]
        public void UnitApiGetCities_When_ListContainsOne_Then_ReturnCountOne()
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };            
            
            var result = controller.GetCities() as OkNegotiatedContentResult<IEnumerable<City>>;

            Assert.That(result.Content.ToList().Count == 1);           
        }

        [Test]
        [TestCase(1)]
        public void UnitApiGetCityById_WhenCityIdContainedInList_Then_ReturnThatCity(int id)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };        
                        
            var result = controller.Get(id) as OkNegotiatedContentResult<City>;

            Assert.That(result.Content.Id == id);
        }

        [Test]
        [TestCase("Name1")]
        public void UnitApiGetCityByName_WhenCityNameContainedInList_Then_ReturnThatCity(string name)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };            

            var result = controller.Get(name) as OkNegotiatedContentResult<City>;

            Assert.That(result.Content.Name == name);
        }

        [Test]
        [TestCase(2)]
        public void UnitApiGetCityById_WhenCityIdNotContainedInList_Then_ReturnNull(int id)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };            

            var result = controller.Get(id) as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("Name2")]
        public void UnitApiGetCityByName_WhenCityNameNotContainedInList_Then_ReturnNull(string name)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };            

            var result = controller.Get(name) as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("Name10")]
        public void UnitApiPostCity_WhenCityNameNotContainedInList_Then_AddToList(string name)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm { City = new City { Name = name } });            

            var result = controller.Post(name) as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(name, cities[1].Name);
            Assert.AreEqual(2, cities.Count);
        }

        [Test]
        [TestCase("Name1")]
        public void UnitApiPostCity_WhenCityNameContainedInList_Then_DontAddToList(string name)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm { City = new City { Name = name } });            

            var result = controller.Post(name) as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, cities.Count);
        }

        [Test]
        [TestCase(1)]
        public void UnitApiDeleteCityById_WhenCityContainedInList_Then_CityCountOneDown(int id)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };            

            var result = controller.Delete(id) as OkResult;

            Assert.AreEqual(0, cities.Count);
        }

        [Test]
        [TestCase(5)]
        public void UnitApiDeleteCityById_WhenCityNotContainedInList_Then_CityCountConstant(int id)
        {
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };                        

            var result = controller.Delete(id) as NotFoundResult;

            Assert.AreEqual(1, cities.Count);
        }
        [Test]        
        public void UnitApiUpdateCity_WhenDataIsValid_Then_UpdateCity()
        {
            int id = 1;
            var city = new City { Id = 1, Name = "Name3" };
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };                        

            var result = controller.Put(id, city) as OkResult;

            Assert.AreEqual(id, cities[0].Id);
            Assert.AreEqual(city.Name, cities[0].Name);
        }

        [Test]        
        public void UnitApiUpdateCity_WhenDataIsInvalid_Then_UpdateCity()
        {
            int id = 5;
            var city = new City { Id = 1, Name = "Name3" };
            cities = new List<City> { new City { Id = 1, Name = "Name1" } };                        

            var result = controller.Put(id, city) as BadRequestResult;
            
            Assert.AreNotEqual(city.Name, cities[0].Name);
        }
    }
}
