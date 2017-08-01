using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UwpSample.Models;

namespace UwpSample.MvvmLight.ViewModels
{
    public class AcademyViewModel : BaseViewModel
    {
        private Academy _academy;
        private INavigationService _navigationService;

        public AcademyViewModel(INavigationService navigationService)
        {
            _academy = new Academy();
            _navigationService = navigationService;

            Title = "Academy";
            SearchCommand = new RelayCommand(search);
            SearchFilter = "";
            Students = new ObservableCollection<Student>();
            search();
        }
        
        private string _searchFilter;
        public string SearchFilter
        {
            get { return _searchFilter; }
            set
            {
                _searchFilter = value;
                RaisePropertyChanged(() => SearchFilter);
            }
        }

        public ObservableCollection<Student> Students { get; private set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                if (_selectedStudent != null)
                {
                    MessengerInstance.Send(_selectedStudent);
                    _navigationService.NavigateTo(nameof(StudentViewModel));
                }

                RaisePropertyChanged(() => SelectedStudent);
            }
        }

        public ICommand SearchCommand { get; set; }

        private void search()
        {
            Students.Clear();
            if (string.IsNullOrWhiteSpace(SearchFilter))
            {
                foreach (var student in _academy.GetAllStudents())
                {
                    Students.Add(student);
                }
            }
            else
            {
                foreach (var student in _academy.GetByStudentName(SearchFilter))
                {
                    Students.Add(student);
                }
                foreach (var student in _academy.GetByStudentCity(SearchFilter))
                {
                    Students.Add(student);
                }
            }
        }
    }
}
