namespace IntroNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Intro.Data;

    public class Program
    {
       public static void Main()
        {
            // P01_SchoolCompetition();

            var context = new SchoolDbContext();
            ClearDatabase(context);
        }

        private static void ClearDatabase(SchoolDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static void P01_SchoolCompetition()
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
