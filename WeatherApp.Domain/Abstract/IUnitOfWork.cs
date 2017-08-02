using System;
using System.Threading.Tasks;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<City> Cities { get; }
        IRepository<HistoryRecord> History { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
