namespace P00DownloadFilesAsync
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
          Task.Run(async () =>
          {
             await DownloadFileAsync();
          })
                .GetAwaiter()
                .GetResult();
        }

        public static async Task DownloadFileAsync()
        {

            Console.WriteLine("Downloading...");

            var webClient = new WebClient();
            var fileUrl = "http://www.albahari.com/threading/";
            
            await webClient.DownloadFileTaskAsync(fileUrl, "book.html");

            Console.WriteLine("Download successfull!");
        }
    }
}
