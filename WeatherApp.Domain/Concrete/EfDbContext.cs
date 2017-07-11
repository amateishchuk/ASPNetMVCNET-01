using System.Collections.Generic;
using System.Data.Entity;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.OwmService;
using WeatherApp.OwmService;

namespace WeatherApp.Domain.Concrete
{
    public class EfDbContext : DbContext
    {
        static EfDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public EfDbContext() : base("name=WeatherOwmDb")
        {
            
        }

        public DbSet<City> FavoriteCities { get; set; }
        public DbSet<HistoryRecord> WeatherHistories { get; set; }
        //public DbSet<DayData> DayDatas { get; set; }
        //public DbSet<Temperature> Tempes { get; set; }

    }

    public class DbInitializer : CreateDatabaseIfNotExists<EfDbContext>
    {
        protected override void Seed(EfDbContext context)
        {
            var kiev = new City {Name = "Kiev"};
            var lviv = new City { Name = "Lviv"};
            var kharkiv = new City { Name = "Kharkiv" };
            var dnipropetrovsk = new City { Name = "Dnipropetrovsk" };
            var odessa = new City { Name = "Odessa" };

            context.FavoriteCities.AddRange(new List<City> {kiev, lviv, kharkiv, dnipropetrovsk, odessa});
            context.SaveChanges();
        }
    }
}