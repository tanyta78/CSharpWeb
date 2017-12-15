namespace IntroNetCore.DbInitialize.Generators
{
    using System;
    using System.Collections.Generic;
    using Intro.Data;
    using Intro.Models;

    public class CourseGenerator
    {
        private static Random rnd = new Random();

        private static string[] courseNames =
        {
            "DB-Advanced",
            "DB-Basics",
            "OOP-Advanced",
            "OOP-Basics",
            "C# Web",
            "Programming Fundamentals",
            "Programming Basic"
        };

        internal static void InitialCourseSeed(SchoolDbContext db, int count)
        {
            var courses = new List<Course>();

            for (int i = 0; i < count; i++)
            {
                DateTime startDate = DateGenerator.GenerateDate();
                DateTime endDAte = DateGenerator.GenerateEndDate(startDate);
                var price = Convert.ToDecimal(rnd.NextDouble() * 600);
                var name = courseNames[rnd.Next(0, courseNames.Length)];

                Course course = new Course()
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate =endDAte ,
                    Price =price,
                    Description = name + " "+startDate.Date.ToShortDateString() +" "+endDAte.Date.ToShortDateString()+" "+Math.Round(price)+ "lv."
                };

               courses.Add(course);
               
            }

            db.Courses.AddRange(courses);
            db.SaveChanges();
        }

       
    }
}
