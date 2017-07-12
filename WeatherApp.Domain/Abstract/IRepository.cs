using System;
using System.Collections.Generic;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Abstract
{
    public interface IRepository<T> where T : class
    {
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}