using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public int Count => throw new NotImplementedException();

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<HistoryRecord>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<HistoryRecord> GetAsync(Expression<Func<HistoryRecord, bool>> predicate)
        {
            throw new NotImplementedException();
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

        public async Task InsertAsync(HistoryRecord item)
        {
            if (await context.WeatherHistories.CountAsync() > 14)
            {
                var firstHistory = await context.WeatherHistories.FirstOrDefaultAsync();
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
