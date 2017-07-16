using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;
using WeatherApp.Tests.Fake;

namespace WeatherApp.Tests
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        private IRepository<City> cities;
        private IRepository<HistoryRecord> history;

        public IRepository<City> Cities
        {
            get
            {
                if (cities == null)
                    cities = new FakeCityRepo();
                return cities;
            }
        }

        public IRepository<HistoryRecord> History
        {
            get
            {
                if (history == null)
                    history = new FakeHistoryRepo();
                return history;
            }
        }

        public void Dispose()
        {
            
        }

        public void SaveChanges()
        {
            //...
        }
    }
}
