using System;

namespace LinnenWebApp_v1.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Student(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public Student()
        {

        }
    }
}