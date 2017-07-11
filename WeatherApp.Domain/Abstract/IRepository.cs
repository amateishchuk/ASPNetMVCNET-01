using System;
using System.Collections.Generic;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Abstract
{
    public interface IRepository : IDisposable
    {
        void AddToFavorites(string name);
        void AddLastRequestToWeatherHistory(HistoryRecord dayHistory);
        void UpdateExists(City city);
        void DeleteFromFavorites(int id);
        City GetCityById(int id);
        IEnumerable<City> FavoriteCities { get; }
        IEnumerable<HistoryRecord> LastRequests { get; }
    }
}