using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Domain.Abstract
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Func<T, bool> predicate);
        T Find(Func<T, bool> predicate);
        void Insert(T item);
        void Update(T item);
        void Delete(T item);
        T First { get; }
        int Count { get; }
    }
}
