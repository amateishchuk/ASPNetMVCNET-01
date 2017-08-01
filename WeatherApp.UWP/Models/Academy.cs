using System;
using System.Collections.Generic;
using System.Linq;

namespace UwpSample.Models
{
    public class Academy
    {
        private List<Student> _students;

        public Academy()
        {
            _students = new List<Student>
            {
                new Student
                {
                    FirstName = "Ben",
                    LastName = "Jonson",
                    City = "London",
                    BirthDate = new DateTime(1990, 12, 21)
                },
                new Student
                {
                    FirstName = "John",
                    LastName = "Berkly",
                    City = "London",
                    BirthDate = new DateTime(1992, 1, 11)
                },
                new Student
                {
                    FirstName = "Andrey",
                    LastName = "Ivanod",
                    City = "Kyiv",
                    BirthDate = new DateTime(1993, 2, 2)
                },
                new Student
                {
                    FirstName = "Oleg",
                    LastName = "Romanenko",
                    City = "Dnipro",
                    BirthDate = new DateTime(1989, 1, 7)
                },
                new Student
                {
                    FirstName = "Svetlana",
                    LastName = "Moiseenko",
                    City = "Lviv",
                    BirthDate = new DateTime(1994, 12, 21)
                },
                new Student
                {
                    FirstName = "Vitaly",
                    LastName = "Ilchenko",
                    City = "Lviv",
                    BirthDate = new DateTime(1965, 8, 2)
                },
            };
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _students.OrderBy(s => s.FirstName).ToList();
        }

        public IEnumerable<Student> GetByStudentName(string filter)
        {
            return GetAllStudents().Where(s => s.FirstName.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase)
                                            || s.LastName.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public IEnumerable<Student> GetByStudentCity(string filter)
        {
            return GetAllStudents().Where(s => s.City.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
    }
}
