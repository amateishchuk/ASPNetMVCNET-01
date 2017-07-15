using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WeatherApp.Domain.Entities;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.OwmService
{
    public class DayData
    {
        public int Id { get; set; }        
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public double Speed { get; set; }
        public int Deg { get; set; }
        public int Clouds { get; set; }
        public virtual Temperature Temp { get; set; }

        [IgnoreDataMember]
        public virtual HistoryRecord HistoryRecord { get; set; }

        [NotMapped]
        public int Dt { get; set; }
        [NotMapped]
        public List<Weather> Weather { get; set; }
        [NotMapped]
        public double? Rain { get; set; }
    }
}
