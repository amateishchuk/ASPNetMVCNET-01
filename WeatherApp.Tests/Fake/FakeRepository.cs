using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.Fake
{
    public class FakeRepository<T> : IRepository<T> where T : class
    {
        public readonly List<T> Data = new List<T>();

        public T First => Data.FirstOrDefault();

        public int Count => Data.Count;

        public void Delete(T item)
        {
            Data.Remove(item);
        }

        public T Find(Func<T, bool> predicate)
        {
            return Data.FirstOrDefault(predicate);
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return Data.Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return Data.ToList();
        }

        public void Insert(T item)
        {
            Data.Add(item);
        }
        public void Update(T item)
        {
            
        }
    }
}
