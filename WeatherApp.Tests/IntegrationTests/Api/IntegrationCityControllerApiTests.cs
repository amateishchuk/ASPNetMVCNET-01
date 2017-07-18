using NUnit.Framework;
using System;
using System.Web.Http.Results;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.Controllers.Api;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.IntegrationTests.Api
{
    [TestFixture]
    public class IntegrationCityControllerApiTests
    {
        private readonly IUnitOfWork unitOfwork;
        private readonly IWeatherService weatherService;
        private readonly CityController controller;

        public IntegrationCityControllerApiTests()
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKeyOwm"];
            string apiUri = ConfigurationManager.AppSettings["ApiUriOwm"];

            unitOfwork = new UnitOfWork("TestDb");
            weatherService = new WeatherServiceOwm(apiKey, apiUri);
            controller = new CityController(unitOfwork, weatherService);
        }

        [Test]
        public void IntegrationApiGetCities_When_CityListContain1Cities_Then_Return1Records()
        {
            string name1 = "GetCities1";          
           

            unitOfwork.Cities.Insert(new City { Name = name1 });
            unitOfwork.SaveChanges();
            var result = controller.GetCities() as OkNegotiatedContentResult<IEnumerable<City>>;
            var resultCity1 = result.Content.FirstOrDefault(c => c.Name == name1);
            unitOfwork.Cities.Delete(resultCity1);
            unitOfwork.SaveChanges();


            Assert.AreEqual(name1, resultCity1.Name);
            Assert.AreEqual(0, unitOfwork.Cities.GetAll().Count());
        }

        [Test]
        public void IntegrationApiGetCityById_When_CityIdValid_Then_ThatCity()
        {
            string name1 = "GetCityById";            


            unitOfwork.Cities.Insert(new City { Name = name1 });
            unitOfwork.SaveChanges();
            var city = unitOfwork.Cities.Get(c => c.Name == name1);
            var result = controller.Get(city.Id) as OkNegotiatedContentResult<City>;            
            unitOfwork.Cities.Delete(city);
            unitOfwork.SaveChanges();


            Assert.AreEqual(city.Id, result.Content.Id);
            Assert.AreEqual(0, unitOfwork.Cities.GetAll().Count());
        }
        [Test]
        public void IntegrationApiGetCityByName_When_CityNameValid_Then_ThatCity()
        {
            string name1 = "GetCityByName";            


            unitOfwork.Cities.Insert(new City { Name = name1 });
            unitOfwork.SaveChanges();
            var city = unitOfwork.Cities.Get(c => c.Name == name1);
            var result = controller.Get(city.Name) as OkNegotiatedContentResult<City>;
            unitOfwork.Cities.Delete(city);
            unitOfwork.SaveChanges();


            Assert.AreEqual(city.Name, result.Content.Name);
            Assert.AreEqual(0, unitOfwork.Cities.GetAll().Count());
        }
        [Test]
        public void IntegrationApiGetCityById_When_CityIdIncorrect_Then_Badrequest()
        {                                    
            var result = controller.Get(250) as BadRequestResult;

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
        public void IntegrationApiGetCityByName_When_CityNameIncorrect_Then_Badrequest()
        {            
            var result = controller.Get("InvalidCityName") as BadRequestResult;

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
        [Test]
        public void IntegrationApiPostNewCity_When_InputNameIsValid_Then_CitiesRepoMustContainTheCity()
        {
            string name = "Kharkiv";

            var result = controller.Post(name) as OkResult;
            var city = unitOfwork.Cities.Get(c => c.Name == name);
            unitOfwork.Cities.Delete(city);
            unitOfwork.SaveChanges();

            Assert.AreEqual(name, city.Name);
            Assert.IsInstanceOf(typeof(OkResult), result);
        }
        [Test]
        [TestCase(null)]
        [TestCase("PostCity")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("Post123City")]
        public void IntegrationApiPostNewCity_When_InputNameIsValid_Then_CitiesRepoMustContainTheCity(string name)
        {            
            var result = controller.Post(name) as BadRequestErrorMessageResult;

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), result);
        }

        [Test]
        public void IntegrationApiUpdate_When_CityExist_Then_ReturnOk()
        {
            string nameBeforeUpdate = "Kiev";
            string nameAfterUpdate = "Lviv";

            unitOfwork.Cities.Insert(new City { Name = nameBeforeUpdate });
            unitOfwork.SaveChanges();
            var cityBeforeUpdate = unitOfwork.Cities.Get(c => c.Name == nameBeforeUpdate);
            cityBeforeUpdate.Name = nameAfterUpdate;

            var result = controller.Put(cityBeforeUpdate.Id, cityBeforeUpdate) as OkResult;

            var cityAfterUpdate = unitOfwork.Cities.Get(c => c.Name == nameAfterUpdate);
            unitOfwork.Cities.Delete(cityAfterUpdate);
            unitOfwork.SaveChanges();


            Assert.AreEqual(cityBeforeUpdate.Id, cityAfterUpdate.Id);
            Assert.AreEqual(nameAfterUpdate, cityAfterUpdate.Name);
        }

        [Test]
        public void IntegrationApiUpdate_When_CityIDsAreDiff_Then_ReturnBadRequest()
        {
            string name = "Kiev";
            unitOfwork.Cities.Insert(new City { Name = name });
            unitOfwork.SaveChanges();
            var city = unitOfwork.Cities.Get(c => c.Name == name);
            unitOfwork.Cities.Delete(city);
            unitOfwork.SaveChanges();

            var result = controller.Put(city.Id + 1, city) as BadRequestResult;

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public void IntegrationApiUpdate_When_CityModelStateInvalid_Then_ReturnBadRequest()
        {
            var result = controller.Put(1, new City { Id = 1 }) as BadRequestResult;

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
        [Test]
        public void IntegrationApiUpdate_When_CityIdApsend_Then_ReturnBadRequest()
        {
            var result = controller.Put(1000, new City { Id = 1000, Name = "Hello" }) as BadRequestResult;

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
        [Test]
        public void IntegrationApiDelete_When_CityExist_Then_DeleteAndReturnOk()
        {
            string name = "DeleteCity";
            unitOfwork.Cities.Insert(new City { Name = name });
            unitOfwork.SaveChanges();
            var city = unitOfwork.Cities.Get(c => c.Name == name);
            
            var result = controller.Delete(city.Id) as OkResult;

            Assert.IsInstanceOf(typeof(OkResult), result);
            Assert.That(unitOfwork.Cities.Get(c => c.Name == name) == null);
            Assert.That(unitOfwork.Cities.Get(c => c.Id == city.Id) == null);
        }
        [Test]
        public void IntegrationApiDelete_When_CityDoesntExist_Then_ReturnNotFound()
        {
            var result = controller.Delete(588) as NotFoundResult;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }
    }
}
