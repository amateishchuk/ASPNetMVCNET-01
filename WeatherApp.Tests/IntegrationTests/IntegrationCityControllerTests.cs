using NUnit.Framework;
using System.Collections.Generic;
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
        private readonly CityController controller;

        public CityControllerTests()
        {
            unitOfWork = new UnitOfWork("TestDb");
            controller = new CityController(unitOfWork);
        }
        [Test]
        public void IntegrationGetFavourites_When_ListContainsOne_Then_ReturnCountOne()
        {
            var city = new City { Name = "IntegrateCity1" };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();
            
            var result = controller.GetFavorites() as PartialViewResult;
            var resultModel = (result.Model as IEnumerable<City>).ToList();
            var resultCity = resultModel.FirstOrDefault(c => c.Name == city.Name);
            unitOfWork.Cities.Delete(resultCity);
            unitOfWork.SaveChanges();

            Assert.AreEqual(city.Name, resultCity.Name);
        }

        [Test]
        [TestCase("Odessa")]
        public void IntegrationAddCity_When_CityExistsInList_Then_CityCountLeftConstant(string name)
        {
            var city = new City { Name = name };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();
            var foundCity = unitOfWork.Cities.Get(c => c.Name == city.Name);

            controller.Add(city.Name);

            var count = unitOfWork.Cities.GetAll().Where(c => c.Name == city.Name).Count();
            unitOfWork.Cities.Delete(foundCity);
            unitOfWork.SaveChanges();


            Assert.AreEqual(1, count);
        }

        [Test]
        public void IntegrationAddCity_When_CityDoesntExistInList_Then_CityCountUpOne()
        {
            string cityName1 = "Dnipropetrovsk";
            string cityName2 = "Dniprodzerzhynsk ";
            var city = new City { Name = cityName1 };
            var city2 = new City { Name = cityName2 };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();

            controller.Add(cityName2);
            var dniproMatch = unitOfWork.Cities.GetAll().Where(c => c.Name.StartsWith("Dnipro"));
            foreach(var dnipro in dniproMatch)
                unitOfWork.Cities.Delete(dnipro);
            unitOfWork.SaveChanges();

            Assert.AreEqual(2, dniproMatch.Count());
        }

        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("Kukushka112")]
        public void IntegrationAddCity_When_CityNameIncorrect_Then_ReturnNotFoundResult(string city)
        {
            var result = controller.Add(city) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void IntegrationEditCity_When_CityExist_Then_ReturnThatCity()
        {
            var city = new City { Name = "Bor" };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();
            var foundCity = unitOfWork.Cities.Get(c => c.Name == city.Name);

            var result = controller.Edit(foundCity.Id) as ViewResult;
            var model = result.Model as City;
            unitOfWork.Cities.Delete(foundCity);
            unitOfWork.SaveChanges();

            Assert.IsTrue(foundCity.Id == model.Id);
        }

        [Test]
        public void IntegrationEditCity_When_CityDoesntExist_Then_ReturnHttpNotFound()
        {
            var result = controller.Edit(100) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void IntegrationUpdateCity_When_CityExist_Then_ReturnRedirect()
        {
            var city = new City { Name = "Kilo" };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();
            var foundCity = unitOfWork.Cities.Get(c => c.Name == city.Name);

            var result = controller.Edit(foundCity.Id) as ViewResult;
            var model = result.Model as City;
            unitOfWork.Cities.Delete(foundCity);
            unitOfWork.SaveChanges();

            Assert.AreEqual(city.Name, foundCity.Name);
            Assert.AreEqual(foundCity.Id, model.Id);
        }

        [Test]
        public void IntegrationUpdateCity_When_CityDataInvalid_Then_ReturnNotFoundResult()
        {
            var city = new City { Id = 10050 };

            var result = controller.Edit(city) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void IntegrationUpdateCity_When_CityDoesntExist_Then_ReturnNotFoundResult()
        {
            var result = controller.Edit(new City { Id = 11111111, Name = "Buffalo" }) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void IntegrationDeleteCity_When_CityExist_Then_ReturnThatCity()
        {
            var city = new City { Name = "California " };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();
            var foundCity = unitOfWork.Cities.Get(c => c.Name == city.Name);

            var result = controller.Delete(foundCity.Id) as ViewResult;
            var model = result.Model as City;
            unitOfWork.Cities.Delete(foundCity);
            unitOfWork.SaveChanges();

            Assert.IsTrue(foundCity.Id == model.Id);
        }

        [Test]
        public void IntegrationDeleteCity_When_CityDoesntExist_Then_ReturnHttpNotFoundResult()
        {
            var result = controller.Delete(161) as HttpNotFoundResult;

            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void IntegrationDeleteConfirmCity_When_CityExist_Then_ReturnRedirectResult()
        {
            var city = new City { Name = "DeleteConfirm" };
            unitOfWork.Cities.Insert(city);
            unitOfWork.SaveChanges();
            var foundCity = unitOfWork.Cities.Get(c => c.Name == city.Name);

            var result = controller.DeleteConfirm(foundCity.Id) as RedirectToRouteResult;
            int count = unitOfWork.Cities.GetAll().Where(c => c.Name == city.Name).Count();

            Assert.AreEqual(0, count);
        }

        [Test]
        public void IntegrationDeleteConfirmCity_When_CityDoesntExist_Then_ReturnHttpNotFoundResult()
        {

            var result = controller.DeleteConfirm(189) as HttpNotFoundResult;

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

    }
}
