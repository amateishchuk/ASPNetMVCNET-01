namespace WeatherApp.Domain.Concrete
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coordinate coord { get; set; }
        public string country { get; set; }
        public int population { get; set; }
    }
}
