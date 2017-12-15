namespace IntroNetCore.DbInitialize
{
    using System;
    using Generators;
    using Intro.Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseInitializer
    {
        private static Random rnd = new Random();

        public static void ResetDatabase(SchoolDbContext db)
        {

            db.Database.EnsureDeleted();

            db.Database.Migrate();

        }

        public static void InitialSeed(SchoolDbContext db)
        {
            SeedStudents(db, 100);

            SeedCourses(db, 30);

            SeedStudentsCourses(db, 120);

            SeedHomeworks(db, 150);

            SeedResources(db,150);

            SeedLincenses(db);
        }

        private static void SeedLincenses(SchoolDbContext db)
        {
            LicensesGenerator.InitialLicenseSeed(db);
        }

        private static void SeedStudentsCourses(SchoolDbContext db, int count)
        {
            StudentsCoursesGenerator.InitialStudentCoursesSeed(db, count);
        }

        private static void SeedResources(SchoolDbContext db, int count)
        {
            ResouceGenerator.InitialResourseSeed(db,count);
        }

        private static void SeedHomeworks(SchoolDbContext db, int count)
        {
            HomeworkGenerator.InitialHomeworkSeed(db, count);
        }

        private static void SeedCourses(SchoolDbContext db, int count)
        {
            CourseGenerator.InitialCourseSeed(db, count);
        }

        private static void SeedStudents(SchoolDbContext db, int count)
        {
            StudentGenetaror.InitialStudentSeed(db, count);
        }
    }
}
