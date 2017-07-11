using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class EfRepository : IRepository
    {
        bool disposed = false;
        EfDbContext context;

        public EfRepository()
        {
            context = new EfDbContext();
        }
        public void AddToFavorites(string name)
        {
            if (context.FavoriteCities.FirstOrDefault(c => c.Name == name) == null)
            {
                context.FavoriteCities.Add(new City { Name = name });
                context.SaveChanges();
            }
        }

        public void DeleteFromFavorites(int id)
        {
            var city = context.FavoriteCities.FirstOrDefault(c => c.Id == id);

            if (city != null)
            {
                context.FavoriteCities.Remove(city);
                context.SaveChanges();
            }
        }
        public IEnumerable<City> FavoriteCities
        {
            get { return context.FavoriteCities.ToList(); }
        }

        public IEnumerable<HistoryRecord> LastRequests
        {
            get
            {
                return context.WeatherHistories
                    //.Include(h => h.DayData)
                    //.Include(h => h.DayData.Temp)
                        .OrderByDescending(h => h.Id)
                        .ToList();
            }
        }

        public void UpdateExists(City city)
        {
            context.Entry(city).State = EntityState.Modified;
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public City GetCityById(int id)
        {
            return context.FavoriteCities.FirstOrDefault(c => c.Id == id);
        }

        public void AddLastRequestToWeatherHistory(HistoryRecord dayHistory)
        {

            if (context.WeatherHistories.ToList().Count > 15)
            {
                var firstRecord = context.WeatherHistories.FirstOrDefault();
                context.WeatherHistories.Remove(firstRecord);
                context.SaveChanges();
            }

            context.WeatherHistories.Add(dayHistory);
            context.SaveChanges();
        }
    }
}