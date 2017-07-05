using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WeatherApp.Domain.Concrete;

namespace WeatherApp.Services
{
    public class ServiceOWM
    {
        string apiKey;
        string adressMask;

        public ServiceOWM()
        {
            apiKey = "9585125f4eafa5d5a356d38b0c453c91";
            adressMask = "http://api.openweathermap.org/data/2.5/forecast/daily?q={city}&units=metric&cnt={qtyDays}&APPID={apiKey}";
        }
        public WeatherOWM GetWeatherInfo(string city, int qtyDays)
        {
            var generatedLink = generateLink(city, qtyDays);
            var responseString = new WebClient().DownloadString(generatedLink);

            return JsonConvert.DeserializeObject<WeatherOWM>(responseString);
        }
        private string generateLink(string city, int qtyDays)
        {
            var generatedLink = adressMask
                .Replace("{city}", city)
                .Replace("{qtyDays}", qtyDays.ToString())
                .Replace("{apiKey}", apiKey);

            return generatedLink;
        }
    }
}