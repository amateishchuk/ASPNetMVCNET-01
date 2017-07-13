using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Tests.Fake;

namespace WeatherApp.Tests
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public void SetRepository<T>(IRepository<T> repository) where T : class
        {
            _repositories[typeof(T)] = repository;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            object repository;
            return _repositories.TryGetValue(typeof(T), out repository)
                       ? (IRepository<T>)repository
                       : new FakeRepository<T>();
        }

        public void SaveChanges()
        {

        }
    }
}
