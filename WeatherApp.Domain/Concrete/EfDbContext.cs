﻿using System.Collections.Generic;
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


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayData>()
                .HasRequired(d => d.Temp)
                .WithRequiredPrincipal(t => t.DayData)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<HistoryRecord>()
                .HasRequired(r => r.DayData)
                .WithRequiredPrincipal(d => d.HistoryRecord)
                .WillCascadeOnDelete(true);

                

            base.OnModelCreating(modelBuilder);
        }

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