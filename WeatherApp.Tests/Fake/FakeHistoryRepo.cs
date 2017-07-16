using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Tests.Fake
{
    public class FakeHistoryRepo : IRepository<HistoryRecord>
    {
        List<HistoryRecord> histories = new List<HistoryRecord>();

        public void Delete(int id)
        {
            var history = histories.FirstOrDefault(h=>h.Id == id);
            if (history != null)
                histories.Remove(history);
        }

        public HistoryRecord Get(Func<HistoryRecord, bool> predicate)
        {
            return histories.FirstOrDefault(predicate);
        }

        public IEnumerable<HistoryRecord> GetAll()
        {
            return histories.OrderBy(h => h.Id).ToList();
        }

        public void Insert(HistoryRecord item)
        {
            histories.Add(item);
        }

        public void Update(HistoryRecord item)
        {
            var history = histories.FirstOrDefault(c => c.Id == item.Id);
            if (history != null)
            {
                histories.Remove(history);
                histories.Add(item);
            }
        }
    }
}
