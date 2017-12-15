namespace IntroNetCore.DbInitialize.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Intro.Data;
    using Intro.Models;

    public class ResouceGenerator
    {
        private static Random rnd = new Random();

        private static string[] resourceNames =
        {
            "Data Structures - AA-Trees and AVL Trees",
            "Data Structures - Basic Tree Data Structures",
            "Data Structures - Binary Search Trees",
            "Data Structures - B-Trees and Red-Black Trees",
            "Data Structures - Combining Data Structures",
            "Data Structures - Exercises Linear Data Structures",
            "Data Structures - Hash Tables",
            "Data Structures - Heaps and Priority Queue",
            "Data Structures - Linear Data Structures",
            "Data Structures - Quad Trees",
            "Data Structures - Rope and Trie",
            "Алгоритми - Recursion",
            "Алгоритми - Combinatorial Algorithms",
            "Алгоритми - Greedy Algorithms",
            "Алгоритми - Dynamic Programming",
            "Алгоритми - Graphs and Graph Algorithms",
            "Алгоритми - Advanced Graph Algorithms",
            "Алгоритми - Problem Solving Methodology"
        };

        private static ResourceType[] types =
        {
            ResourceType.Presentation,
            ResourceType.Video,
            ResourceType.Document,
            ResourceType.Other
        };

        public static void InitialResourseSeed(SchoolDbContext db,int count)
        {
            var coursesIds = db
                .Courses
                .Select(c => c.Id)
                .ToArray();

            var resources = new List<Resource>();

            for (int i = 0; i < count; i++)
            {
                var name = resourceNames[rnd.Next(0, resourceNames.Length)];
                var url = "D:\\Resources\\" + name;
                var resourceType = types[rnd.Next(0, types.Length)];
                var courseId = coursesIds[rnd.Next(1, coursesIds.Length)];

                var resource = new Resource()
                {
                    Name = name,
                    Url = url,
                    ResourceType = resourceType,
                    CourseId = courseId
                };

                resources.Add(resource);

            }

            db.Resources.AddRange(resources);
            db.SaveChanges();

        }
    }
}