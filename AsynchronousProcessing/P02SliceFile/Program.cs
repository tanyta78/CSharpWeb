﻿namespace P02SliceFile
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            var videoPath = Console.ReadLine();
            var destination = Console.ReadLine();
            int pieces = int.Parse(Console.ReadLine());

            SliceAsync(videoPath, destination, pieces);

            Console.WriteLine("Anything else?");
            while (true)
            {
                Console.ReadLine();
            }

        }

        private static void SliceAsync(string sourceFile, string destinationPath, int pieces)
        {
            Task.Run((() =>
            {
                Slice(sourceFile, destinationPath, pieces);
            }));
        }

        private static void Slice(string sourceFile, string destinationPath, int pieces)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (var source = new FileStream(sourceFile, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(sourceFile);

                var partLength = (source.Length / pieces) + 1;
                var currentByte = 0;

                for (int currentPart = 1; currentPart <= pieces; currentPart++)
                {
                    string filePath = String.Format("{0}/Part-{1}{2}", destinationPath, currentPart, fileInfo.Extension);
                    using (var destination = new FileStream(filePath, FileMode.Create))
                    {

                        byte[] buffer = new byte[partLength];
                        while (currentByte <= partLength * currentPart)
                        {
                            int readBytesCount = source.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }
                    }

                }
                
                Console.WriteLine("Slice complete.");

            }
        }
    }
}
