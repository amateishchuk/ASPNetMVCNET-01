using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.OwmService;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Entities
{
    public class HistoryRecord
    {
        public HistoryRecord() { }
        public HistoryRecord(WeatherOwm weatherResult)
        {
            City = weatherResult.City.Name;
            DateTime = DateTime.Now;
            DayData = weatherResult.List[0];
        }

        
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime DateTime { get; set; }
        public virtual DayData DayData { get; set; }
        
    }
}
