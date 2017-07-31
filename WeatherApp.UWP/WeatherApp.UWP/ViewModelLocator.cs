using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using WeatherApp.UWP.ViewModels;
using WeatherApp.UWP.Views;

namespace WeatherApp.UWP
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new NavigationService();
            navigationService.Configure(nameof(WeatherViewModel), typeof(WeatherView));

            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<WeatherViewModel>();
        }

        public WeatherViewModel WeatherVMInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WeatherViewModel>();
            }
        }
    }
}
