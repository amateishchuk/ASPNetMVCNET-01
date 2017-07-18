using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        [Required]        
        public string Name { get; set; }


        [NotMapped]        
        public virtual Coordinate Coord { get; set; }        
        [NotMapped]
        public string Country { get; set; }        
        [NotMapped]
        public int Population { get; set; }

        public static bool IsValidName(string name)
        {
            if (name != null)
            {
                string city = name.Trim();
                if (!string.IsNullOrEmpty(city) && name.All(n => Char.IsLetter(n) || n == ' '))
                    return true;
            }
            return false;
        }
    }
}
