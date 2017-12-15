namespace IntroNetCore.DbInitialize.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Intro.Data;
    using Intro.Models;

    public class LicensesGenerator
    {
        private static Random rnd = new Random();

        private static string[] LicensesNames =
        {
            "MIT",
            "ApacheLicense",
            "GNU General Public License v3.0",
            "BSD 2 clause",
            "BSD 3 clause",
            "Eclipse Public License",
            "GNU General Public License v2.0",
            "GNU Lesser General Public License v3.0",
            "GNU Lesser General Public License v2.0",
            "Mozilla Public License v3.0",
            "TheUnlicense",

        };


        public static void InitialLicenseSeed(SchoolDbContext db)
        {
            var licenses = new List<License>();

            var resourseIds = db
                .Resources
                .Select(r => r.Id)
                .ToArray();

            for (int i = 0; i < LicensesNames.Length; i++)
            {
                var license = new License()
                {
                    Name = LicensesNames[i],
                    ResourceId = resourseIds[rnd.Next(1, resourseIds.Length)]
                };

                licenses.Add(license);

            }

            db.Licenses.AddRange(licenses);
            db.SaveChanges();
        }
    }
}