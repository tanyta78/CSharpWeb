namespace P03SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            int port = 1337;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ipAddress, port);
            tcpListener.Start();


            Task
                .Run(async () =>
                    {
                        await Connect(tcpListener);
                    })
                .GetAwaiter()
                .GetResult();

        }

        private static async Task Connect(TcpListener tcpListener)
        {
            while (true)
            {
                //connecting
                var client = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");

                //read request
                byte[] buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length);

                var clientMessage = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(clientMessage.Trim('\0'));

                //sending
                byte[] responseMessage = Encoding.UTF8.GetBytes("Hello from my server!");
                await client.GetStream().WriteAsync(responseMessage, 0, responseMessage.Length);

                client.Dispose();
            }






        }
    }
}
