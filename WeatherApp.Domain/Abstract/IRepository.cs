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
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        T Get(Func<T, bool> predicate);
    }
}
