using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_Lab7
{
    public class Student : IComparable<Student>
    {
        private string name;
        private string school;
        private int year;

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ім'я не може бути порожнє");
                name = value;
            }
        }

        public string School
        {
            get { return school; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Школа не може бути порожня");
                school = value;
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                    throw new ArgumentException("Некоректний рік народження");
                year = value;
            }
        }

        public Student(string name, string school, int year)
        {
            Name = name;
            School = school;
            Year = year;
        }

        public int CompareTo(Student? other)
        {
            if (other == null) return 1;
            return Year.CompareTo(other.Year);
        }

        public override string ToString()
        {
            return $"Ім'я: {Name}, Школа: {School}, Рік : {Year}";
        }
    }
}
