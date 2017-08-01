using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UwpSample.MvvmLight.ViewModels;
using UwpSample.MvvmLight.Views;

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

            var navigationService = new NavigationService();
            navigationService.Configure(nameof(AcademyViewModel), typeof(AcademyView));
            navigationService.Configure(nameof(StudentViewModel), typeof(StudentView));

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }

            //Register your services used here
            //SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<AcademyViewModel>();
            SimpleIoc.Default.Register<StudentViewModel>();
            
            ServiceLocator.Current.GetInstance<StudentViewModel>();
        }


        // <summary>
        // Gets the Academy view model.
        // </summary>
        // <value>
        // The Academy view model.
        // </value>
        public AcademyViewModel AcademyVMInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AcademyViewModel>();
            }
        }

        // <summary>
        // Gets the Student view model.
        // </summary>
        // <value>
        // The Student view model.
        // </value>
        public StudentViewModel StudentVMInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StudentViewModel>();
            }
        }

        // <summary>
        // The cleanup.
        // </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
