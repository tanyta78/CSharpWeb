using System.Runtime.CompilerServices;
using System.Text;
using IntroNetCore.DbInitialize;
using Microsoft.EntityFrameworkCore;

namespace IntroNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Intro.Data;
    using Intro.Models;

    public class Program
    {
        
        public static void Main()
        {
            // P00_SchoolCompetition();

            var context = new SchoolDbContext();
            //   DatabaseInitializer.ResetDatabase(context);
            // Console.WriteLine("Database created!");

            //  DatabaseInitializer.InitialSeed(context);
            // Console.WriteLine("Database seeded!");

            //P01_ListAllStudents(context);
            // P02_ListAllCourses(context);
            // P03_ListAllCoursesWithMoreThan5Resourses(context);
            //Console.WriteLine("Please insert date!");
            //var date = Console.ReadLine();
            //P04_ListAllCoursesWithMoreThan5Resourses(context, date);
            // P05_ListAllStudentInfo(context);
            // P06_ListAllCoursesWithResourses(context);
            // P07_ListAllStudentInfo(context);
            var tab = new int[15];
           var result =  tab.Length;
            Console.WriteLine(result);
        }
        

        private static void P07_ListAllStudentInfo(SchoolDbContext context)
        {
            var students = context.Students
                                  .Include(s => s.CourseParticipateIn)
                                  .ThenInclude(sc => sc.Course)
                                  .ThenInclude(c => c.Resources)
                                  .ThenInclude(r => r.Licenses)
                                  .Select(s => new
                                  {
                                      Name = s.Name,
                                      NumberOfCourses = s.CourseParticipateIn.Count,
                                      TotalResourses = s.CourseParticipateIn.Sum(cs => cs.Course.Resources.Count),
                                      TotalLicenses = s.CourseParticipateIn.Sum(cs => cs.Course.Resources.Sum(r => r.Licenses.Count))
                                  })
                                  .OrderByDescending(s => s.NumberOfCourses)
                                  .ThenByDescending(s => s.TotalResourses)
                                  .ThenBy(s => s.Name)
                                  .ToList();

            var sb = new StringBuilder();

            foreach (var student in students)
            {
                sb.AppendLine($"Student: {student.Name} TotalCourses : {student.NumberOfCourses} Total Resourses: {student.TotalResourses} Total Licenses: {student.TotalLicenses}");

            }

            Console.Write(sb.ToString());
        }

        private static void P06_ListAllCoursesWithResourses(SchoolDbContext context)
        {
            var courses = context.Courses
                                 .Include(c => c.Resources)
                                 .ThenInclude(r => r.Licenses)
                                 .Select(c => new
                                 {
                                     Name = c.Name,
                                     ResourcesNames = c.Resources.Select(r => new
                                     {
                                         resourceName = r.Name,
                                         licencesNames = r.Licenses.Select(l => new
                                         {
                                             licenseName = l.Name
                                         }).ToList()
                                     }).ToList()

                                 })
                                 .OrderByDescending(c => c.ResourcesNames.Count())
                                 .ThenBy(c => c.Name)
                                 .ToList();


            var sb = new StringBuilder();

            foreach (var c in courses)
            {
                sb.AppendLine($"Course: {c.Name}" + Environment.NewLine + $"==Resourses: ");
                var resourses = c.ResourcesNames.OrderByDescending(r => r.licencesNames.Count)
                                 .ThenBy(r => r.resourceName);

                if (resourses.Any())
                {
                    foreach (var resourse in resourses)
                    {
                        var licenses = resourse.licencesNames;
                        sb.AppendLine($"===={resourse.resourceName}");
                        if (licenses.Any())
                        {
                            sb.AppendLine($"======Licenses:{string.Join("||", licenses)}");
                        }
                        else
                        {
                            sb.AppendLine("======Licenses:none");
                        }
                    }
                }
                else
                {
                    sb.AppendLine("none");
                }

                Console.Write(sb.ToString());
            }

        }


        private static void P05_ListAllStudentInfo(SchoolDbContext context)
        {
            var students = context.Students
                                  .Include(s => s.CourseParticipateIn)
                                  .ThenInclude(sc => sc.Course)
                                  .Select(s => new
                                  {
                                      Name = s.Name,
                                      NumberOfCourses = s.CourseParticipateIn.Count,
                                      TotalPrice = (decimal)s.CourseParticipateIn.Sum(cp => cp.Course.Price),
                                      AveragePrice = s.CourseParticipateIn.Count == 0 ? 0 : (decimal)s.CourseParticipateIn.Sum(sc => sc.Course.Price) /
                                                     s.CourseParticipateIn.Count
                                  })
                .OrderByDescending(s => s.TotalPrice)
                .ThenByDescending(s => s.NumberOfCourses)
                .ThenBy(s => s.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var student in students)
            {
                sb.AppendLine($"Student: {student.Name} Courses Enrolled In: {student.NumberOfCourses} Total Price: {student.TotalPrice:f2} Average Price: {student.AveragePrice:f2}");

            }

            Console.Write(sb.ToString());
        }

        private static void P04_ListAllCoursesWithMoreThan5Resourses(SchoolDbContext context, string date)
        {
            DateTime.TryParse(date, out var activeDate);
            var courses = context.Courses
                .Include(c => c.Participants)
                .Where(c => c.EndDate < activeDate)
                .Select(c => new
                {
                    Name = c.Name,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Duration = c.EndDate.Subtract(c.StartDate),
                    StudentsEnrolled = c.Participants.Count
                })
                .OrderByDescending(c => c.StudentsEnrolled)
                .ThenByDescending(c => c.Duration);

            var sb = new StringBuilder();

            foreach (var course in courses)
            {
                sb.AppendLine($"Course: {course.Name} StartDate: {course.StartDate} EndDate:  {course.EndDate} Duration: {course.Duration} Participants:  {course.StudentsEnrolled}");

            }

            Console.Write(sb.ToString());
        }

        private static void P03_ListAllCoursesWithMoreThan5Resourses(SchoolDbContext context)
        {
            var courses = context.Courses
                .Include(c => c.Resources)
                .Where(c => c.Resources.Count > 5)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new
                {
                    Name = c.Name,
                    ResoursesCount = c.Resources.Count
                });

            var sb = new StringBuilder();

            foreach (var course in courses)
            {
                sb.AppendLine($"Course: {course.Name} Resourses Count: {course.ResoursesCount}");

            }

            Console.Write(sb.ToString());
        }

        private static void P02_ListAllCourses(SchoolDbContext context)
        {
            var courses = context.Courses
                .Include(c => c.Resources)
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .Select(c => new
                {
                    Name = c.Name,
                    Description = c.Description,
                    Resourses = c.Resources.Select(r => $"{r.Name} {r.ResourceType} {r.Url}")
                });

            var sb = new StringBuilder();

            foreach (var course in courses)
            {
                sb.AppendLine($"{course.Name}" + " " + $"{course.Description}" + Environment.NewLine + $"{string.Join(" || ", course.Resourses)}");

            }

            Console.Write(sb.ToString());
        }

        private static void P01_ListAllStudents(SchoolDbContext context)
        {
            var students = context.Students
                .Include(s => s.Homeworks)
                .Select(s => new
                {
                    Name = s.Name,
                    Submissions = s.Homeworks.Select(h => new
                    {
                        Content = h.Content,
                        Type = h.ContentType
                    })
                });

            var sb = new StringBuilder();

            foreach (var student in students)
            {
                sb.AppendLine($"Name:{student.Name}");
                sb.AppendLine($"Homework submissions:");
                var submissionsText = "none";
                if (student.Submissions.Count() != 0)
                {
                    submissionsText = string.Join(Environment.NewLine, student.Submissions);

                }
                sb.AppendLine(submissionsText);

            }
            Console.Write(sb.ToString());
        }

        private static void ClearDatabase(SchoolDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static void P00_SchoolCompetition()
        {
            var studentsScores = new Dictionary<string, int>();
            var studentsCategories = new Dictionary<string, HashSet<string>>();

            while (true)
            {
                var line = Console.ReadLine();

                if (line == "END")
                {
                    break;
                }

                var tokens = line.Split(' ');
                var studentName = tokens[0];
                var category = tokens[1];
                var points = int.Parse(tokens[2]);

                if (!studentsCategories.ContainsKey(studentName))
                {
                    studentsCategories.Add(studentName, new HashSet<string>());
                }

                studentsCategories[studentName].Add(category);

                if (!studentsScores.ContainsKey(studentName))
                {
                    studentsScores.Add(studentName, 0);
                }

                studentsScores[studentName] += points;

            }

            var orderedStudentsByPoints = studentsScores.OrderByDescending(sc => sc.Value).ThenBy(sc => sc.Key);

            foreach (var studentPoints in orderedStudentsByPoints)
            {
                var studentCategories = studentsCategories[studentPoints.Key].OrderBy(sc => sc);
                Console.WriteLine($"{studentPoints.Key}: {studentPoints.Value} [{string.Join(", ", studentCategories)}]");
            }
        }
    }
}
