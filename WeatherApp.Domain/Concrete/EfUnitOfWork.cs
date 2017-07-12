using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private EfDbContext context;
        private IRepository<City> cityRepo;
        private IRepository<HistoryRecord> historyRepo;

        public EfUnitOfWork(string connectionString)
        {
            context = new EfDbContext(connectionString);
        }

        public IRepository<City> Cities
        {
            get
            {
                if (cityRepo == null)
                    cityRepo = new CityRepository(context);
                return cityRepo;
            }
        }

        public IRepository<HistoryRecord> HistoryRecords
        {
            get
            {
                if (historyRepo == null)
                    historyRepo = new HistoryRecordRepository(context);
                return historyRepo;
            }
        }

        public void Save()
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
