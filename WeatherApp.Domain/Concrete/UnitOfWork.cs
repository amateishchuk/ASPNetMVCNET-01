using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext context;
        private IRepository<City> cities;
        private IRepository<HistoryRecord> history;
        
        public UnitOfWork(string connectionString)
        {
            context = new EfDbContext(connectionString);
        }

        public IRepository<City> Cities
        {
            get
            {
                if (cities == null)
                    cities = new CityRepository(context);                    
                return cities;
            }
        }

        public IRepository<HistoryRecord> History
        {
            get
            {
                if (history == null)
                    history = new HistoryRepository(context);
                return history;
            }
        }        

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
