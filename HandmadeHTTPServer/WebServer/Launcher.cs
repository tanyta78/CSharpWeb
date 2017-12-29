namespace WebServer
{
    using Application;
    using Server;
    using Server.Contracts;
    using Server.Routing;

    public class Launcher:IRunnable
   {
       public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var mainApp = new MainApplication();
            var routeConfig = new AppRouteConfig();
            mainApp.Configure(routeConfig);
            
            var webServer=new WebServer(1337,routeConfig);
            webServer.Run();

        }
    }
}
