using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<City> Cities { get; }
        IRepository<HistoryRecord> HistoryRecords { get; }
        void Save();
    }
}
