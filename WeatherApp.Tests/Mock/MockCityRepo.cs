using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.Mock
{
    public static class MockCityRepo
    {
        static Mock<IRepository<City>> mockCityRepo = new Mock<IRepository<City>>();
        public static List<City> cities;


        public static Mock<IRepository<City>> Create(List<City> list)
        {
            cities = list;
            setup();
            return mockCityRepo;
        }

        private static void setup()
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
        }
    }
}
