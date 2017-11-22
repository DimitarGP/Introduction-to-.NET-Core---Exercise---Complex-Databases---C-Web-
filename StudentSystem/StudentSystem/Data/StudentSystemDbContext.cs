using StudentSystem.Models;

namespace StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;

    public class StudentSystemDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<License> Licenses { get; set; }    

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=True");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder
                .Entity<Student>()
                .HasMany(s => s.Courseses)
                .WithOne(c => c.Student)
                .HasForeignKey(s => s.StudentId);

            builder
                .Entity<Student>()
                .HasMany(s => s.HomeworksSubmissions)
                .WithOne(hw => hw.Student)
                .HasForeignKey(hw => hw.StudenId);

            builder
                .Entity<Course>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId);

            builder
                .Entity<Course>()
                .HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);

            builder
                .Entity<Course>()
                .HasMany(c => c.HomeworksSubmissions)
                .WithOne(hw => hw.Course)
                .HasForeignKey(hw => hw.CourseId);

            builder
                .Entity<Resource>()
                .HasMany(r => r.Licenses)
                .WithOne(l => l.Resource)
                .HasForeignKey(l => l.ResourseId);

            base.OnModelCreating(builder);
        }
    }
}
