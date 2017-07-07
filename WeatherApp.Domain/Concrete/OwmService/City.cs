namespace WeatherApp.Domain.Concrete
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Coordinate Coord { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
    }
}
