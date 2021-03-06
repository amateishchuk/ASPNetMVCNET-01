﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WeatherApp.Domain.OwmService;

namespace WeatherApp.OwmService
{
    public class Temperature
    {
        public int Id { get; set; }
        public double Day { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Night { get; set; }
        public double Eve { get; set; }
        public double Morn { get; set; }

        [IgnoreDataMember]
        public virtual DayData DayData { get; set; }
    }
}
