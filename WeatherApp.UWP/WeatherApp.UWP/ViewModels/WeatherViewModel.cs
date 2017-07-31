using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.UWP.Models;
using WeatherApp.UWP.Services;
using Windows.UI.Popups;

namespace WeatherApp.UWP.ViewModels
{
    public class WeatherViewModel : ViewModelBase
    {
        private WeatherService _weatherService;
        public Weather WeatherData { get; set; }
        public ICommand Command { get; set; }
        public string City { get; set; }
        public int QtyDays { get; set; } = 1;

        public WeatherViewModel()
        {            
            _weatherService = new WeatherService();
            Command = new RelayCommand(GetWeather);

            WeatherData = null;            
        }

        public async void GetWeather()
        {
            try
            {
                WeatherData = await _weatherService.GetWeather(City, QtyDays);
                RaisePropertyChanged(() => WeatherData);
            }
            catch (Exception ex)
            {
                var popup = new MessageDialog(ex.Message);
                await popup.ShowAsync();
            }
        }
    }
}
