namespace IntroNetCore.DbInitialize.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Intro.Data;
    using Intro.Models;

    public class HomeworkGenerator
    {
        private static Random rnd = new Random();

        private static string[] contents =
        {
            "First homework",
            "Second homework",
            "Third homework",
            "Forth homework",
            "Fifth homework",
            "Sixth homework"
        };

       private static ContentType[] types = new ContentType[]
        {
            ContentType.Application,
            ContentType.Pdf,
            ContentType.Zip
        };

        public static void InitialHomeworkSeed(SchoolDbContext db, int count)
        {
            var homeworks = new List<Homework>();

            for (int i = 0; i < count; i++)
            {
                var homework = new Homework()
                {
                    Content = contents[rnd.Next(0, contents.Length)],
                SubmissionDate = DateGenerator.GenerateDate(),
                    ContentType = types[rnd.Next(0, types.Length)],
                    StudentId = GetRandomStudentFromDb(),
                    CourseId = GetRandomCourseFromDb()
                };

                homeworks.Add(homework);
               
            }

            db.Homeworks.AddRange(homeworks);
            db.SaveChanges();
            
        }

        private static int GetRandomCourseFromDb()
        {
            using (var db = new SchoolDbContext())
            {
                var coursesIds = db
                    .Courses
                    .Select(c => c.Id)
                    .ToArray();

                return coursesIds[rnd.Next(0, coursesIds.Length)];
            }
        }

        private static int GetRandomStudentFromDb()
        {
            using (var db = new SchoolDbContext())
            {
                var studentsIds = db
                    .Students
                    .Select(s => s.Id)
                    .ToArray();

                return studentsIds[rnd.Next(0, studentsIds.Length)];
            }
        }
    }
}