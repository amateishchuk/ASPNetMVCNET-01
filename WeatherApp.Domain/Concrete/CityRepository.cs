using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class CityRepository : IRepository<City>
    {
        private EfDbContext context;

        public CityRepository(EfDbContext ctx)
        {
            context = ctx;
        }
        public void Add(City city)
        {
            if (context.Cities.FirstOrDefault(c => c.Name == city.Name) == null)
                context.Cities.Add(city);
        }

        public void Delete(int id)
        {
            City city = Get(id);
            if (city != null)
                context.Cities.Remove(city);
        }

        public IEnumerable<City> Find(Func<City, bool> predicate)
        {
            return context.Cities.Where(predicate).ToList();
        }

        public City Get(int id)
        {
            return context.Cities.Find(id);
        }

        public IEnumerable<City> GetAll()
        {
            return context.Cities.OrderBy(c => c.Name).ToList();
        }

        public void Update(City item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
