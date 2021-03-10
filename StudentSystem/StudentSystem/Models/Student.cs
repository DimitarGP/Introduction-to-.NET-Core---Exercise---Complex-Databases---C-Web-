using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace StudentSystem.Models
{
    public class Student
    {
        public Student()
        {
            this.Courseses = new List<StudentCourse>();
            this.HomeworksSubmissions = new List<Homework>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegestrationDate { get; set; }

        public DateTime? Birthday { get; set; }

        public List<StudentCourse> Courseses { get; set; }

        public List<Homework> HomeworksSubmissions { get; set; }

    }
}
