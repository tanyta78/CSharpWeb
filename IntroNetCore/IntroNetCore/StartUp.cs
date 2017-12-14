using System.Runtime.CompilerServices;
using System.Text;
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
            ClearDatabase(context);
            Console.WriteLine("Database created!");
            // P01_ListAllStudents(context);
            //P02_ListAllCourses(context);
            //P03_ListAllCoursesWithMoreThan5Resourses(context);
           // var date = Console.ReadLine();
            //P04_ListAllCoursesWithMoreThan5Resourses(context,date);
           // P05_ListAllStudentInfo(context);


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
                    TotalPrice = s.CourseParticipateIn.Sum(sc => sc.Course.Price),
                    AveragePrice = (double) s.CourseParticipateIn.Sum(sc => sc.Course.Price) /
                                   s.CourseParticipateIn.Count
                })
                .OrderByDescending(s=>s.TotalPrice)
                .ThenByDescending(s=>s.NumberOfCourses)
                .ThenBy(s=>s.Name);

            var sb = new StringBuilder();

            foreach (var student in students)
            {
                sb.AppendLine($"Student: {student.Name} CourseNumberEnrolledIn {student.NumberOfCourses} Total Price {student.TotalPrice} Average Price{student.AveragePrice}");

            }

            Console.Write(sb.ToString());
        }

        private static void P04_ListAllCoursesWithMoreThan5Resourses(SchoolDbContext context,string date)
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
                sb.AppendLine($"Course: {course.Name} StartDate{course.StartDate} EndDate {course.EndDate} Duration{course.Duration} Participants {course.StudentsEnrolled}");

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
                sb.AppendLine($"Course: {course.Name} Resourses Count{course.ResoursesCount}");

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
                    Resourses = c.Resources
                });

            var sb = new StringBuilder();

            foreach (var course in courses)
            {
                sb.AppendLine($"{course.Name}" + Environment.NewLine + $"{course.Description}");

                var resoursesText = course.Resourses.Select(r => $"{r.Name} {r.ResourceType} {r.Url}");

                sb.AppendLine($"{string.Join(Environment.NewLine, resoursesText)}");
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
                if (student.Submissions.Count()!=0)
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
