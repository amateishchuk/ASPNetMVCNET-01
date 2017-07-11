using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.OwmService;

namespace WeatherApp.Domain.Entities
{
    public class HistoryRecord
    {
        public int Id { get; set; }
        public string City { get; set; }
        //public int? DayDataId { get; set; }
        public virtual DayData DayData { get; set; }
        public DateTime DateTime { get; set; }
    }
}
