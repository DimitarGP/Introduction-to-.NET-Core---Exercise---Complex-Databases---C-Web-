using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Remotion.Linq.Clauses;
using StudentSystem.Data;
using StudentSystem.Models;

namespace StudentSystem
{
    public class Program
    {
        private static Random random = new Random();

        static void Main()
        {
            using (var db = new StudentSystemDbContext())
            {
                db.Database.Migrate();

                //SeedInitialData(db);
                //SeedLicensData(db);
                //PrintStudentsWithHomework(db);
                //PrintCoursesWithResources(db);
                //PrintCourseWithMoreThanFiveResource(db);
                //PrintCourseActionOnADate(db);
                //PrintStudentAndPrices(db);
                //PrintCourseWithResourceAndLicense(db);
                PrintStudentWithCourseAndResourseAndLicense(db);
            }
        }

        private static void SeedInitialData(StudentSystemDbContext db)
        {
            const int totalStudent = 25;
            const int totalCourse = 10;
            DateTime currentDate = DateTime.Now;

            //Students
            for (int i = 0; i < totalStudent; i++)
            {
                db.Add(new Student
                {
                    Name = $"Student {i}",
                    RegestrationDate = currentDate.AddDays(i),
                    Birthday = currentDate.AddYears(-20).AddDays(i),
                    PhoneNumber = $"RandomNumber {i}"
                });
            }

            db.SaveChanges();

            //Course

            var addedCourses = new List<Course>();

            for (int i = 0; i < totalCourse; i++)
            {
                Course course = new Course
                {
                    Name = $"Course{i}",
                    Description = $"Description Details {i}",
                    Price = 100 * i,
                    StartDate = currentDate.AddDays(i),
                    EndDate = currentDate.AddDays(i + 20)
                };

                addedCourses.Add(course);

                db.Courses.Add(course);
            }

            db.SaveChanges();

            //Students in Coourse
            List<int> studentIds = db
                .Students
                .Select(s => s.Id)
                .ToList();

            for (int i = 0; i < totalCourse; i++)
            {
                Course currentCourse = addedCourses[i];
                int studentsInCourse = random.Next(2, totalStudent / 2);
                for (int j = 0; j < studentsInCourse; j++)
                {
                    var studenId = studentIds[random.Next(0, studentIds.Count)];
                    if (!currentCourse.Students.Any(s => s.StudentId == studenId ))
                    {
                        currentCourse.Students.Add(new StudentCourse
                        {
                            StudentId = studenId
                        });
                    }
                    else
                    {
                        j--;
                    }
                }

                var resourseInCourse = random.Next(2, 20);
                var type = new[] {0, 1, 2, 999};

                for (int j = 0; j < resourseInCourse; j++)
                {
                    currentCourse.Resources.Add(new Resource
                    {
                        Name = $"Resourse {j}",
                        URL = $"Url {j}",
                        Type = (TypeOfResource)type[random.Next(0, type.Length)]
                    });
                }
            }

            db.SaveChanges();

            //Homeworks

            for (int i = 0; i < totalCourse; i++)
            {
                var currentCourse = addedCourses[i];
                var studentsInCourseIds = currentCourse
                    .Students
                    .Select(s => s.StudentId)
                    .ToList();
                for (int j = 0; j < studentsInCourseIds.Count; j++)
                {
                    var totalHomework = random.Next(2, 5);

                    for (int k = 0; k < totalHomework; k++)
                    {
                        db.Homeworks.Add(new Homework
                        {
                            Content = $"Content Homework {i}",
                            SubmissionDate = currentDate.AddDays(-i),
                            ContentType = ContentType.Zip,
                            StudenId = studentsInCourseIds[j],
                            CourseId = currentCourse.Id
                        });
                    }
                }
                db.SaveChanges();
            }
        }

        private static void SeedLicensData(StudentSystemDbContext db)
        {
            var resourceIds = db
                .Resources
                .Select(r => r.Id)
                .ToList();
            for (int i = 0; i < resourceIds.Count; i++)
            {
                var totalLicenses = random.Next(1, 4);

                for (int j = 0; j < totalLicenses; j++)
                {
                    db.Licenses.Add(new License
                    {
                        Name = $"License {i} {j}",
                        ResourseId = resourceIds[i]
                    });
                }
            }
            db.SaveChanges();
        }

        private static void PrintStudentsWithHomework(StudentSystemDbContext db)
        {
            var result = db
                .Students
                .Select(s => new
                {
                    s.Name,
                    Homeworks = s.HomeworksSubmissions.Select(h => new {h.Content, h.ContentType})
                })
                .ToList();

            foreach (var student in result)
            {
                Console.WriteLine(student.Name);
                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine($"---{homework.Content} - {homework.ContentType}");
                }
            }
        }

        private static void PrintCoursesWithResources(StudentSystemDbContext db)
        {
            var result = db
                .Courses
                .OrderBy(c => c.StartDate)
                .ThenBy(c => c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.Description,
                    Resources = c.Resources.Select(r => new
                    {
                        r.Name,
                        r.Type,
                        r.URL
                    })
                })
                .ToList();

            foreach (var course in result)
            {
                Console.WriteLine($"{course.Name} - {course.Description}");
                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"{resource.Name} - {resource.Type} - {resource.URL}");
                }
            }

        }

        private static void PrintCourseWithMoreThanFiveResource(StudentSystemDbContext db)
        {
            var result = db
                .Courses
                .Where(c => c.Resources.Count > 5)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new
                {
                    c.Name,
                    Resources = c.Resources.Count
                })
                .ToList();
            foreach (var course in result)
            {
                Console.WriteLine($"{course.Name} - {course.Resources}");
            }

        }

        private static void PrintCourseActionOnADate(StudentSystemDbContext db)
        {
            var date = DateTime.Now.AddDays(25);
            var result = db
                .Courses
                .Where(c => c.StartDate < date && date < c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.StartDate,
                    c.EndDate,
                    Duration = c.EndDate.Subtract(c.StartDate),
                    Students = c.Students.Count
                })
                .OrderByDescending(c => c.Students)
                .ThenByDescending(c => c.Duration)
                .ToList();

            foreach (var course in result)
            {
                Console.WriteLine(
                    $"{course.Name} - {course.StartDate.ToShortDateString()} - {course.EndDate.ToShortDateString()}");
                Console.WriteLine($"---Duration - {course.Duration.TotalDays}");
                Console.WriteLine($"---Students - {course.Students}");
                Console.WriteLine($"---------");
            }
        }

        private static void PrintStudentAndPrices(StudentSystemDbContext db)
        {
            var results = db
                .Students
                .Where(c => c.Courseses.Any())
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courseses.Count,
                    TotalPrice = s.Courseses.Sum(c => c.Course.Price),
                    AveragePrice = s.Courseses.Average(c => c.Course.Price)
                })
                .OrderByDescending(s => s.Courses)
                .ThenBy(s => s.Name)
                .ToList();

            foreach (var student in results)
            {
                Console.WriteLine(
                    $"{student.Name} - {student.Courses} - {student.TotalPrice} -{student.AveragePrice}");
            }
        }

        private static void PrintCourseWithResourceAndLicense(StudentSystemDbContext db)
        {
            var result = db
                .Courses
                .OrderByDescending(c => c.Resources.Count)
                .ThenBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Resources = c
                        .Resources
                        .OrderByDescending(r => r.Licenses.Count)
                        .ThenBy(r => r.Name)
                        .Select(r => new
                        {
                            r.Name,
                            Licenses = r.Licenses.Select(l => l.Name)
                        })
                });

            foreach (var course in result)
            {
                Console.WriteLine(course.Name);
                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"---{resource.Name}");
                    foreach (var license in resource.Licenses)
                    {
                        Console.WriteLine($"------{license}");
                    }
                }
            }
        }

        private static void PrintStudentWithCourseAndResourseAndLicense(StudentSystemDbContext db)
        {
            var result = db
                .Students
                .Where(s => s.Courseses.Any())
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courseses.Count,
                    Resourses = s.Courseses.Sum(c => c.Course.Resources.Count),
                    Licenses = s.Courseses.Sum(c => c.Course.Resources.Select(r => r.Licenses).Count()),
                })
                .ToList();

            foreach (var st in result)
            {
                Console.WriteLine(
                    $"{st.Name} - {st.Courses} - {st.Resourses} - {st.Licenses}");
            }
        }
    }
}

