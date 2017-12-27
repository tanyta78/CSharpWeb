namespace P01EvenNumberThread
{
    using System;
    using System.Linq;
    using System.Threading;

    public class Program
    {
        static void Main()
        {
            var input = Console.ReadLine().Split().Select(int.Parse).ToArray();
            var start = input[0];
            var end = input[1];
            
            Thread evens = new Thread((() => PrintEventNumbers(start,end)));
            evens.Start();
            evens.Join();
            Console.WriteLine("Thread finished work");
        }

        private static void PrintEventNumbers(int start, int end)
        {
            if (start%2!=0)
            {
                start++;
            }
            
            for (int i = start; i <= end; i+=2)
            {
                Console.WriteLine(i);
            }
        }
    }
}
