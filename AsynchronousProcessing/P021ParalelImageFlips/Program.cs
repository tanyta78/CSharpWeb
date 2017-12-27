﻿namespace P021ParalelImageFlips
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    public class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(currentDirectory + "\\Images");
            var files = directoryInfo.GetFiles();

            const string resultDir = "Result";
            
            if (Directory.Exists(resultDir))
            {
                Directory.Delete(resultDir,true);
            }

            Directory.CreateDirectory(resultDir);

            var tasks = new List<Task>();
            
            foreach (var file in files)
            {
               var task = Task.Run(() =>
                {
                    var image = Image.FromFile(file.FullName);
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    image.Save($"{resultDir}\\flip-{file.Name}");

                    Console.WriteLine($"{file.Name} processed...");
                });
                
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Finished");
        }
    }
}
