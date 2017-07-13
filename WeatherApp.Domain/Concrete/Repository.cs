using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        DbContext dataContext;
        DbSet<T> dataSet;

        public T First => dataSet.FirstOrDefault();

        public int Count => dataSet.Count();

        public Repository(DbContext dc)
        {
            dataContext = dc;
            dataSet = dc.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dataSet.ToList();
        }
        public void Insert(T item)
        {
            dataSet.Add(item);
        }
        public void Update(T item)
        {
            dataContext.Entry(item).State = EntityState.Modified;
        }
        public void Delete(T item)
        {
            dataSet.Remove(item);
        }        
        public T Get(int id)
        {
            return dataSet.Find(id);
        }
        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return dataSet.Where(predicate).ToList();
        }

        public T Find(Func<T, bool> predicate)
        {
            return dataSet.FirstOrDefault(predicate);
        }
    }
}


//if (item is City)
//{
//    City newCity = item as City;

//    // checking if db doesn't contain city with specified name
//    if (dataContext.Set<City>().FirstOrDefault(c => c.Name.Equals(newCity.Name)) != null)
//        return;
//}
//else if (item is HistoryRecord)
//{
//    HistoryRecord record = item as HistoryRecord;

//    if (dataContext.Set<HistoryRecord>().Count() > 15)
//    {
//        // delete firstRecord (max qty record must be no more than 15)
//        // Then we have max 15 records all time
//        HistoryRecord theOldestRecord = dataContext.Set<HistoryRecord>().FirstOrDefault();                    
//        dataContext.Set<HistoryRecord>().Remove(theOldestRecord);
//    }
//}
