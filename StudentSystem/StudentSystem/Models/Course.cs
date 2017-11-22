using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Models
{
    public class Course
    {
        public Course()
        {
            this.Students = new List<StudentCourse>();
            this.Resources = new List<Resource>();
            this.HomeworksSubmissions = new List<Homework>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public List<StudentCourse> Students { get; set; }

        public List<Resource> Resources { get; set; }

        public List<Homework> HomeworksSubmissions { get; set; }

    }
}
