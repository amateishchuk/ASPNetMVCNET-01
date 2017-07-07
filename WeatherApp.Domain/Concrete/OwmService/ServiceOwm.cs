using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;

namespace WeatherApp.Services
{
    public class ServiceOwm : IWeather
    {
        string apiKey;
        string apiUri;

        public ServiceOwm()
        {
            apiKey = WebConfigurationManager.AppSettings["ApiKeyOWM"];
            apiUri = WebConfigurationManager.AppSettings["ApiUriOWM"];
        }
        public WeatherOwm GetWeatherInfo(string city, int qtyDays)
        {
            var generatedLink = generateLink(city, qtyDays);
            var httpClient = new HttpClient();

            var responseString = httpClient.GetStringAsync(generatedLink).Result;

            return JsonConvert.DeserializeObject<WeatherOwm>(responseString);
        }
        private string generateLink(string city, int qtyDays)
        {
            var generatedLink = apiUri
                .Replace("{city}", city)
                .Replace("{qtyDays}", qtyDays.ToString())
                .Replace("{apiKey}", apiKey);

            return generatedLink;
        }
    }
}