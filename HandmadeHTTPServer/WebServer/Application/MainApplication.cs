namespace WebServer.Application
{
    using Controllers;
    using Server.Contracts;
   using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get("/", request => new HomeController().Index());

            appRouteConfig.Get("/testsession",req=>new HomeController().SessionTest(req));

            appRouteConfig.Get("/users/{?<name>[a-z]+}", request => new HomeController().Index());

        }
    }
}
