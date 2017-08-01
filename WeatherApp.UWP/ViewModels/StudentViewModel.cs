using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UwpSample.Models;

namespace UwpSample.MvvmLight.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        private Student _model;
        private INavigationService _navigationService;

        public StudentViewModel(INavigationService navigationService)
        {
            _model = new Student();
            _navigationService = navigationService;

            Title = "Student Details";

            GoBackCommand = new RelayCommand(goBack);

            MessengerInstance.Register<Student>(this, student =>
            {
                _model = student;
                RaisePropertyChanged(() => FirstName);
                RaisePropertyChanged(() => LastName);
                RaisePropertyChanged(() => BirthDate);
                RaisePropertyChanged(() => City);
            });
        }
        
        public string FirstName => _model.FirstName;

        public string LastName => _model.LastName;

        public string City => _model.City;
        
        public DateTime BirthDate => _model.BirthDate;

        public ICommand GoBackCommand { get; set; }

        private void goBack()
        {
            _navigationService.GoBack();
        }
    }
}
