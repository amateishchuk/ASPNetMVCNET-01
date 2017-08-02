using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        public int Count => context.Cities.Count();

        public async Task<int> CountAsync()
        {
            return await context.Cities.CountAsync();
        }

        public void Delete(City city)
        {
           context.Cities.Remove(city);
        }

        public City Get(Func<City, bool> predicate)
        {
            return context.Cities.FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await context.Cities.ToListAsync();
        }

        public IEnumerable<City> GetAll()
        {
            return context.Cities.ToList();
        }

        public async Task<City> GetAsync(Expression<Func<City, bool>> predicate)
        {
            return await context.Cities.FirstOrDefaultAsync(predicate);
        }

        public void Insert(City item)
        {
            var city = context.Cities.FirstOrDefaultAsync(c => c.Name == item.Name);
            if (city == null)
                context.Cities.Add(item);
        }

        public async void Update(City item)
        {
            var cityInDb = await context.Cities.FindAsync(item.Id);
            
            if (cityInDb != null)
            {
                context.Entry(cityInDb).CurrentValues.SetValues(item);
                context.Entry(cityInDb).State = EntityState.Modified;                
            }            
        }

        public async Task InsertAsync(City item)
        {
            var city = await context.Cities.FirstOrDefaultAsync(c => c.Name == item.Name);
            if (city == null)
                context.Cities.Add(item);
        }
    }
}
