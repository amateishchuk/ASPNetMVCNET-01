using System.ComponentModel.DataAnnotations.Schema;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }



        [NotMapped]
        public Coordinate Coord { get; set; }
        [NotMapped]
        public string Country { get; set; }
        [NotMapped]
        public int Population { get; set; }
    }
}
