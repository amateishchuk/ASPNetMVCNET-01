using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WeatherApp.Controllers.Api;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.UnitTests.Api
{
    [TestFixture]
    public class CityControllerTests
    {
        //City city;
        //List<City> cities;
        //Mock<IUnitOfWork> mockUnitOfWork;
        //Mock<IWeatherService> mockWeatherService;
        //Mock<IRepository<City>> mockCityRepo;

        //public CityControllerTests()
        //{               
        //    cities = new List<City>();
        //    mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockWeatherService = new Mock<IWeatherService>();
        //    mockCityRepo = new Mock<IRepository<City>>();
        //}

        //[SetUp]
        //public void TestSetup()
        //{
        //    mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
        //    city = new City { Id = 1, Name = "Name1" };
        //    cities.Add(city);
        //    mockCityRepo.Setup(m => m.GetAll()).Returns(cities);
        //    mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>())).Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
        //    mockCityRepo.Setup(r => r.Insert(It.IsAny<City>())).Callback((City c) => 
        //    {
        //        var city = cities.FirstOrDefault(ct => ct.Name == c.Name);
        //        if (city == null)
        //            cities.Add(c);
        //    });
        //    mockCityRepo.Setup(m => m.Delete(It.Is<int>(i => i > 0))).Callback((int id) =>
        //    {
        //        var city = cities.FirstOrDefault(c => c.Id == id);
        //        if (city != null)
        //            cities.Remove(city);
        //    });

        //    mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
        //    mockUnitOfWork.Setup(u => u.Cities.GetAll()).Returns(cities);
        //    mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>())).Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
        //    mockUnitOfWork.Setup(u => u.Cities.Insert(It.IsAny<City>())).Callback((City c) => mockCityRepo.Object.Insert(c));
        //    mockUnitOfWork.Setup(u => u.Cities.Delete(It.Is<int>(i => i > 0))).Callback((int id) => mockCityRepo.Object.Delete(id));

        //    mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive))).Returns(new OwmService.WeatherOwm { City = new City { Name = "Name3" } });
        //    //mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
        //    //    .Returns(new OwmService.WeatherOwm { City = new City { Name = name } });
        //}
        [Test]
        public void UnitApiGetCities_When_ListContainsOne_Then_ReturnCountOne()
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };                       
            var mockCityRepo = new Mock<IRepository<City>>();            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockCityRepo.Setup(m => m.GetAll()).Returns(cities);
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockUnitOfWork.Setup(u => u.Cities.GetAll()).Returns(mockCityRepo.Object.GetAll());            
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);


            var result = controller.GetCities() as OkNegotiatedContentResult<IEnumerable<City>>;


            Assert.That(result.Content.ToList().Count == 1);           
        }

        [Test]
        [TestCase(1)]
        public void UnitApiGetCityById_WhenCityIdContainedInList_Then_ReturnThatCity(int id)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));            
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);
            
            var result = controller.Get(id) as OkNegotiatedContentResult<City>;

            Assert.That(result.Content.Id == id);
        }

        [Test]
        [TestCase("Name1")]
        public void UnitApiGetCityByName_WhenCityNameContainedInList_Then_ReturnThatCity(string name)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Get(name) as OkNegotiatedContentResult<City>;

            Assert.That(result.Content.Name == name);
        }

        [Test]
        [TestCase(2)]
        public void UnitApiGetCityById_WhenCityIdNotContainedInList_Then_ReturnNull(int id)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Get(id) as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("Name2")]
        public void UnitApiGetCityByName_WhenCityNameNotContainedInList_Then_ReturnNull(string name)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>()))
                .Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Get(name) as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("Name10")]
        public void UnitApiAddCity_WhenCityNameNotContainedInList_Then_AddToList(string name)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Insert(It.IsAny<City>())).Callback((City c) =>
            {
                var city = cities.FirstOrDefault(ct => ct.Name == c.Name);
                if (city == null)
                    cities.Add(c);
            });
            mockUnitOfWork.Setup(u => u.Cities.Insert(It.IsAny<City>())).Callback((City c) => mockCityRepo.Object.Insert(c));
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm { City = new City { Name = name } });
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Post(name) as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(name, cities[1].Name);
            Assert.AreEqual(2, cities.Count);
        }

        [Test]
        [TestCase("Name1")]
        public void UnitApiAddCity_WhenCityNameContainedInList_Then_DontAddToList(string name)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Insert(It.IsAny<City>())).Callback((City c) =>
            {
                var city = cities.FirstOrDefault(ct => ct.Name == c.Name);
                if (city == null)
                    cities.Add(c);
            });
            mockUnitOfWork.Setup(u => u.Cities.Insert(It.IsAny<City>())).Callback((City c) => mockCityRepo.Object.Insert(c));
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm { City = new City { Name = name } });
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Post(name) as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, cities.Count);
        }

        [Test]
        [TestCase(1)]
        public void UnitApiDeleteCityById_WhenCityContainedInList_Then_CityCountOneDown(int id)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>())).Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>())).Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
            mockCityRepo.Setup(m => m.Delete(It.IsAny<City>())).Callback((City city) =>
            {
                cities.Remove(city);
            });
            mockUnitOfWork.Setup(u => u.Cities.Delete(It.IsAny<City>())).Callback((City city) => mockCityRepo.Object.Delete(city));
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Delete(id) as OkResult;

            Assert.AreEqual(0, cities.Count);
        }

        [Test]
        [TestCase(5)]
        public void UnitApiDeleteCityById_WhenCityNotContainedInList_Then_CityCountConstant(int id)
        {
            var cities = new List<City> { new City { Id = 1, Name = "Name1" } };
            var mockCityRepo = new Mock<IRepository<City>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockWeatherService = new Mock<IWeatherService>();
            mockUnitOfWork.Setup(u => u.Cities).Returns(mockCityRepo.Object);
            mockCityRepo.Setup(r => r.Get(It.IsAny<Func<City, bool>>())).Returns((Func<City, bool> predicate) => cities.FirstOrDefault(predicate));
            mockUnitOfWork.Setup(u => u.Cities.Get(It.IsAny<Func<City, bool>>())).Returns((Func<City, bool> predicate) => mockCityRepo.Object.Get(predicate));
            mockCityRepo.Setup(m => m.Delete(It.IsAny<City>())).Callback((City city) =>
            {
                cities.Remove(city);
            });
            mockUnitOfWork.Setup(u => u.Cities.Delete(It.IsAny<City>())).Callback((City city) => mockCityRepo.Object.Delete(city));
            CityController controller = new CityController(mockUnitOfWork.Object, mockWeatherService.Object);

            var result = controller.Delete(id) as NotFoundResult;

            Assert.AreEqual(1, cities.Count);
        }
    }
}
