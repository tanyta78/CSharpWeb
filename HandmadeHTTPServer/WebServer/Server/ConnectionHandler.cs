namespace WebServer.Server
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Common;
    using Handlers;
    using Http;
    using Http.Contracts;
    using Routing.Contracts;

    public class ConnectionHandler
    {
        private readonly Socket client;
        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            MyValidator.ThrowIfNull(client,nameof(client));
            MyValidator.ThrowIfNull(serverRouteConfig,nameof(serverRouteConfig));
            
            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest!=null)
            {
                var httpContext = new HttpContext(httpRequest);

                var httpResponse = new HttpHandler(this.serverRouteConfig).Handle(httpContext);

                var responseBytes = Encoding.UTF8.GetBytes(httpResponse.ToString());
                
                var byteSegments = new ArraySegment<byte>(responseBytes);

                await this.client.SendAsync(byteSegments, SocketFlags.None);

                Console.WriteLine("====REQUEST====");
                Console.WriteLine(httpRequest);
                Console.WriteLine("====RESPONSE====");
                Console.WriteLine(httpResponse);
                Console.WriteLine();
            }
            
            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<HttpRequest> ReadRequest()
        {
            var request = new StringBuilder();
            
            var data = new ArraySegment<byte>(new byte[1024]);
          

            while (true)
            {
                int numBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numBytesRead==0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numBytesRead);

                request.Append(bytesAsString);
                
                if (numBytesRead<1023)
                {
                    break;
                }
                
            }

            if (request.Length==0)
            {
                return null;
            }
            
            return new HttpRequest(request.ToString());
        }
    }
}
