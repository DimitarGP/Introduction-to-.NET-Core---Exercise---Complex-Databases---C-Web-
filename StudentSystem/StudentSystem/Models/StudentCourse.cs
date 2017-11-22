﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace StudentSystem.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
