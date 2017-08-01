using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.UWP.Models
{
    public class DayData
    {
        public int Id { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public double Speed { get; set; }
        public int Deg { get; set; }
        [NotMapped]
        public string SpeedDeg => $"{this.Speed} m/s {this.Deg}°";
        public int Clouds { get; set; }
        public Temperature Temp { get; set; }
        public int Dt { get; set; }
    }
}
