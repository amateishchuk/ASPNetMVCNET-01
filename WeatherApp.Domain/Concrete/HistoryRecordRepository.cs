using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Concrete
{
    public class HistoryRecordRepository : IRepository<HistoryRecord>
    {
        private EfDbContext context;
        public HistoryRecordRepository(EfDbContext context)
        {
            this.context = context;
        }
        public void Add(HistoryRecord record)
        {
            if (context.WeatherHistories.ToList().Count > 15)
            {
                var firstRecord = context.WeatherHistories.FirstOrDefault();
                context.WeatherHistories.Remove(firstRecord);
            }
            context.WeatherHistories.Add(record);
        }

        public void Delete(int id)
        {
            HistoryRecord record = Get(id);
            if (record != null)
                context.WeatherHistories.Remove(record);
        }

        public IEnumerable<HistoryRecord> Find(Func<HistoryRecord, bool> predicate)
        {
            return context.WeatherHistories.Where(predicate).ToList();
        }

        public HistoryRecord Get(int id)
        {
            return context.WeatherHistories.Find(id);
        }

        public IEnumerable<HistoryRecord> GetAll()
        {
            return context.WeatherHistories
                .OrderByDescending(h => h.Id)
                .ToList();
        }

        public void Update(HistoryRecord item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
