namespace IntroNetCore.DbInitialize.Generators
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Intro.Data;
    using Intro.Models;

    public class StudentsCoursesGenerator
    {
        private static Random rnd = new Random();

        private static List<string> sownPairs = new List<string>();

        internal static void InitialStudentCoursesSeed(SchoolDbContext db, int count)
        {
            var allStudentsIds = db.Students.Select(s => s.Id).ToArray();
            var allCoursesIds = db.Courses.Select(c => c.Id).ToArray();

            var studentCourses = new List<StudentCourse>();

            for (int i = 0; i < count; i++)
            {
                var studentId = allStudentsIds[rnd.Next(0, allStudentsIds.Length)];
                var courseId = allCoursesIds[rnd.Next(0, allCoursesIds.Length)];
                var studentCourse = new StudentCourse()
                {
                    StudentId =studentId ,
                    CourseId = courseId
                };

                var currentPairToSeed = studentId.ToString() + courseId.ToString();

                // Ensures that there is no another Student with same Course
                if (sownPairs.Contains(currentPairToSeed))
                {
                    i--;
                    continue;
                }

                sownPairs.Add(currentPairToSeed);
                studentCourses.Add(studentCourse);
                db.Students.Find(studentId).CourseParticipateIn.Add(studentCourse);
                db.Courses.Find(courseId).Participants.Add(studentCourse);
            }

            db.StudentsCourseses.AddRange(studentCourses);
            db.SaveChanges();
        }


    }
}
