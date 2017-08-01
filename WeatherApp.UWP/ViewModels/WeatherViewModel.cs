using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
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
        private WeatherService weatherService;

        public Weather WeatherData { get; set; }
        public ICommand WeatherCommand { get; set; }

        public string City { get; set; }
        public int QtyDays { get; set; }

        public WeatherViewModel()
        {
            weatherService = new WeatherService();
            WeatherCommand = new RelayCommand(GetWeather);
            QtyDays = 1;
        }

        public async void GetWeather()
        {
            try
            {
                WeatherData = await weatherService.GetWeather(City, QtyDays);
                RaisePropertyChanged(() => WeatherData);
            }
            catch (Exception ex)
            {
                var modal = new MessageDialog(ex.Message);
                await modal.ShowAsync();
            }
        }

    }
}
