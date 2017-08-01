using System;

namespace UwpSample.Models
{
    public class Student
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string City { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
