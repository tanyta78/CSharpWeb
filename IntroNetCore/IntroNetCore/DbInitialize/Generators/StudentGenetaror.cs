namespace IntroNetCore.DbInitialize.Generators
{
    using System;
    using System.Collections.Generic;
    using Intro.Data;
    using Intro.Models;
   
    public class StudentGenetaror
    {
        public static void InitialStudentSeed(SchoolDbContext db, int count)
        {
            var students = new List<Student>();

            for (int i = 0; i < count; i++)
            {
                string name = NameGenerator.FirstName() + " " + NameGenerator.LastName();
                DateTime registeredOn = DateGenerator.GenerateDate();
                string phoneNumber = PhoneNumberGenerator.NewPhoneNumber();

                var student = new Student()
                {
                    Name = name,
                    PhoneNumber = phoneNumber,
                    RegisteredOn = registeredOn
                };

                students.Add(student);
            }

            db.Students.AddRange(students);
        }
    }
}