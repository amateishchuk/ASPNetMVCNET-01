using System.Collections.Generic;

namespace WeatherApp.Domain.Concrete
{
    public class WeatherDayData
    {
        public int dt { get; set; }
        public Temperature temp { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
        public List<Weather> weather { get; set; }
        public double speed { get; set; }
        public int deg { get; set; }
        public int clouds { get; set; }
        public double? rain { get; set; }
    }
}
