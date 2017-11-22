using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSystem.Models
{
    public class Resource
    {
        public Resource()
        {
            this.Licenses = new List<License>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public TypeOfResource Type { get; set; }

        public string URL { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public List<License> Licenses { get; set; } 

    }
}
