using Newtonsoft.Json;
using System;
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
        [JsonProperty("list")]
        public virtual DayData DayData { get; set; }
        
    }
}
