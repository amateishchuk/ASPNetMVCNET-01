using System;
using System.Collections.Generic;
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

        public CityRepository(EfDbContext context)
        {
            this.context = context;
        }

        public void Delete(int id)
        {
            var city = context.Cities.FirstOrDefault(c => c.Id == id);
            if (city != null)
                context.Cities.Remove(city);
        }

        public City Get(int id)
        {
            return context.Cities.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<City> GetAll()
        {
            return context.Cities.OrderBy(c => c.Name).ToList();
        }

        public void Insert(City item)
        {
            var city = context.Cities.FirstOrDefault(c => c.Name == item.Name);
            if (city == null)
                context.Cities.Add(item);
        }

        public void Update(City item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
