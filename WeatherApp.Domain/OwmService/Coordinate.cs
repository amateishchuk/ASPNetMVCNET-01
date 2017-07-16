using System.Runtime.Serialization;
using WeatherApp.Domain.Entities;

namespace WeatherApp.OwmService
{
    public class Coordinate
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }


        [IgnoreDataMember]
        public virtual City City { get; set; }
    }
}
