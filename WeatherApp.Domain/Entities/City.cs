using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        [Required]        
        public string Name { get; set; }


                
        public virtual Coordinate Coord { get; set; }        
        public string Country { get; set; }        
        public int Population { get; set; }
    }
}
