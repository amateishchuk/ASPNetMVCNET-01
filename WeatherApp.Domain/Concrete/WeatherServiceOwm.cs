﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Configuration;
using WeatherApp.Domain.Abstract;
using WeatherApp.OwmService;
using System.Threading.Tasks;

namespace WeatherApp.Domain.Concrete
{
    public class WeatherServiceOwm : IWeatherService
    {
        string apiKey;
        string apiUri;

        public WeatherServiceOwm(string apiKey, string apiUri)
        {
            this.apiKey = apiKey;
            this.apiUri = apiUri;
        }
        

        public WeatherOwm GetWeather(string city, int qtyDays)
        {
            if (String.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException();
            else if (qtyDays < 1 || qtyDays > 16)
                throw new ArgumentOutOfRangeException();


            var generatedLink = generateLink(city, qtyDays);
            try
            {
                var httpClient = new HttpClient();

                var responseString = httpClient.GetStringAsync(generatedLink).Result;

                return JsonConvert.DeserializeObject<WeatherOwm>(responseString);
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public async Task<WeatherOwm> GetWeatherAsync(string city, int qtyDays)
        {
            if (String.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException();
            else if (qtyDays < 1 || qtyDays > 16)
                throw new ArgumentOutOfRangeException();

            string responseString = null;
            var generatedLink = generateLink(city, qtyDays);
            
            using (var http = new HttpClient())
            {
                responseString = await http.GetStringAsync(generatedLink);
            }
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }        
    }
}