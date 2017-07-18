using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeatherApp.Controllers;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.UnitTests
{
    [TestFixture]
    public class UnitCityControllerTests
    {
        private readonly Mock<IRepository<City>> mockCityRepo;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IWeatherService> mockWeatherService;
        private readonly CityController controller;
        private List<City> cities;

        public UnitCityControllerTests()
        {
            cities = new List<City>();
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
            mockWeatherService.Setup(w => w.GetWeather(It.Is<string>(c => c == null), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Throws<ArgumentNullException>();
            mockWeatherService.Setup(w => w.GetWeather(It.Is<string>(c => string.IsNullOrEmpty(c) || string.IsNullOrWhiteSpace(c)), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Throws<ArgumentException>();
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.Is<int>(qty => qty < 1 || qty > 16)))
                .Throws<ArgumentOutOfRangeException>();
        }


        [Test]
        public void UnitGetFavourites_When_ListContainsOne_Then_ReturnCountOne()
        {
            var city = new City { Id = 1, Name = "Name1" };

            cities.Add(city);
            var result = controller.GetFavorites() as PartialViewResult;
            var resultModel = (result.Model as IEnumerable<City>).ToList();
            var flag = resultModel.Contains(city);
            cities.Remove(city);
                       
            Assert.IsTrue(flag);
        }

        [Test]
        [TestCase("ExistCity")]
        public void UnitAddCity_When_CityExistsInList_Then_CityCountLeftConstant(string name)
        {
            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm { City = new City { Name = name } });
            var city = new City { Id = 1, Name = name };
            cities.Add(city);
            
            controller.Add(city.Name);

            var count = cities.Where(c => c.Name == city.Name).Count();
            cities.Remove(city);


            Assert.AreEqual(1, count);
        }

        [Test]
        public void UnitAddCity_When_CityDoesntExistInList_Then_CityCountUpOne()
        {
            string cityName1 = "City1";
            string cityName2 = "City2";
            var city = new City { Id = 1, Name = cityName1 };
            var city2 = new City { Id = 2, Name = cityName2 };

            mockWeatherService.Setup(w => w.GetWeather(It.IsRegex("[A-z]"), It.IsInRange<int>(1, 16, Range.Inclusive)))
                .Returns(new OwmService.WeatherOwm { City = new City { Name = city2.Name } });
            cities.Add(city);

            controller.Add(cityName2);
            var count = cities.Where(c => c.Name.StartsWith("City")).Count();
            cities.Remove(city);
            cities.Remove(city2);


            Assert.AreEqual(2, count);            
        }    
        
        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void UnitAddCity_When_CityNameIncorrect_Then_ReturnNotFoundResult(string city)
        {
            var result = controller.Add(city) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UnitEditCity_When_CityExist_Then_ReturnThatCity()
        {
            var city = new City { Id = 1, Name = "EditCity" };
            cities.Add(city);

            var result = controller.Edit(1) as ViewResult;
            var model = result.Model as City;
            cities.RemoveAll(c => c.Name == city.Name);

            Assert.IsTrue(city.Id == model.Id);
        }

        [Test]
        public void UnitEditCity_When_CityDoesntExist_Then_ReturnHttpNotFound()
        {
            var result = controller.Edit(100) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UnitUpdateCity_When_CityExist_Then_ReturnRedirect()
        {
            var cityBeforeUpdate = new City { Id = 100, Name = "Before" };
            var cityAfter = new City { Id = 100, Name = "After" };
            cities.Add(cityBeforeUpdate);

            var result = controller.Edit(cityAfter) as RedirectToRouteResult;
            cities.RemoveAll(c => c.Id == 100);

            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void UnitUpdateCity_When_CityDataInvalid_Then_ReturnNotFoundResult()
        {
            var city = new City { Id = 150 };

            var result = controller.Edit(city) as HttpNotFoundResult;            

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UnitUpdateCity_When_CityDoesntExist_Then_ReturnNotFoundResult()
        {
            var city = new City { Id = 153 };
            cities.Add(city);

            var result = controller.Edit(new City { Id = 55}) as HttpNotFoundResult;
            cities.RemoveAll(c => c.Id == city.Id);

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UnitDeleteCity_When_CityExist_Then_ReturnThatCity()
        {
            var city = new City { Id = 158, Name = "Delete" };
            cities.Add(city);

            var result = controller.Delete(city.Id) as ViewResult;
            var model = result.Model as City;
            cities.RemoveAll(c => c.Id == city.Id);

            Assert.AreEqual(city.Name, model.Name);
        }

        [Test]
        public void UnitDeleteCity_When_CityDoesntExist_Then_ReturnHttpNotFoundResult()
        {
            var result = controller.Delete(161) as HttpNotFoundResult;
            
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UnitDeleteConfirmCity_When_CityExist_Then_ReturnRedirectResult()
        {
            var city = new City { Id = 179, Name = "DeleteConfirm" };
            cities.Add(city);

            var result = controller.DeleteConfirm(179) as RedirectToRouteResult;
            int count = cities.RemoveAll(c => c.Id == city.Id);

            Assert.AreEqual(0, count);
        }

        [Test]
        public void UnitDeleteConfirmCity_When_CityDoesntExist_Then_ReturnHttpNotFoundResult()
        {

            var result = controller.DeleteConfirm(189) as HttpNotFoundResult;

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }        
    }
}
