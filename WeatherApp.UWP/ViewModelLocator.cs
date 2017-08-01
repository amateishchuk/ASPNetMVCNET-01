using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using WeatherApp.UWP.ViewModels;

namespace UwpSample.MvvmLight
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
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
