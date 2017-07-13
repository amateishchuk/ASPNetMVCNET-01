using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;

namespace WeatherApp.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext dataContext;
        
        public UnitOfWork(string connectionString)
        {
            dataContext = new EfDbContext(connectionString);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(dataContext);
        }

        public void SaveChanges()
        {
            dataContext.SaveChanges();
        }
    }
}
