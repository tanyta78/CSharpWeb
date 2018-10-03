﻿namespace WebServer.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Contracts;
    using Routing;
    using Routing.Contracts;

    public class WebServer:IRunnable
    {
        private const string localHostIpAddress = "127.0.0.1";
        private readonly int port;
        private readonly IServerRouteConfig serverRouteConfig;
        private readonly TcpListener tcpListener;
        private bool isRunning;
        
        public WebServer(int port,IAppRouteConfig routeConfig)
        {
            this.port = port;
            this.tcpListener=new TcpListener(IPAddress.Parse(localHostIpAddress),port);
            this.serverRouteConfig = new ServerRouteConfig(routeConfig);
        }
        
        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server running on {localHostIpAddress}: {this.port}");

           Task.Run(this.ListenLoop).Wait();
           
        }

        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                var client = await this.tcpListener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client,this.serverRouteConfig);
                await connectionHandler.ProcessRequestAsync();
               
            }
        }
    }
}
