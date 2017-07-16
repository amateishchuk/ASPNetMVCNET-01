using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.Fake
{
    public class FakeCityRepo : IRepository<City>
    {
        List<City> cities = new List<City>();
        public void Delete(int id)
        {
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city != null)
                cities.Remove(city);
        }

        public City Get(Func<City, bool> predicate)
        {
            return cities.FirstOrDefault(predicate);
        }

        public IEnumerable<City> GetAll()
        {
            return cities.OrderBy(c=>c.Name).ToList();
        }

        public void Insert(City item)
        {
            var city = cities.FirstOrDefault(c => c.Name == item.Name);
            if (city == null)
                cities.Add(item);
        }

        public void Update(City item)
        {
            var city = cities.FirstOrDefault(c => c.Id == item.Id);
            if (city != null)
            {
                cities.Remove(city);
                cities.Add(item);
            }
        }
    }

}
