using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.OwmService
{
    public class DayData
    {
        public int Id { get; set; }
        //public int? TempId { get; set; }
        public virtual Temperature Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public double Speed { get; set; }
        public int Deg { get; set; }
        public int Clouds { get; set; }



        [NotMapped]
        public int Dt { get; set; }
        [NotMapped]
        public List<Weather> Weather { get; set; }
        [NotMapped]
        public double? Rain { get; set; }
    }
}
