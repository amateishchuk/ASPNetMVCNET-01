using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class HistoryRepository : IRepository<HistoryRecord>
    {
        private EfDbContext context;
        public HistoryRepository(EfDbContext context)
        {
            this.context = context;
        }

        public void Delete(HistoryRecord item)
        {
            context.WeatherHistories.Remove(item);
        }

        public HistoryRecord Get(Func<HistoryRecord, bool> predicate)
        {
            return context.WeatherHistories.FirstOrDefault(predicate);
        }

        public IEnumerable<HistoryRecord> GetAll()
        {
            return context.WeatherHistories.OrderByDescending(h => h.Id).ToList();
        }

        public void Insert(HistoryRecord item)
        {
            if (context.WeatherHistories.Count() > 14)
            {
                var firstHistory = context.WeatherHistories.FirstOrDefault();
                context.WeatherHistories.Remove(firstHistory);
            }

            context.WeatherHistories.Add(item);
        }

        public void Update(HistoryRecord item)
        {
            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
