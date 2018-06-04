namespace WebServer.Application.Controllers
{
    using Server.Enums;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Views.Home;

    public class HomeController
    {
        //GET /
        public IHttpResponse Index()
        {
<<<<<<< HEAD
            return new ViewResponse(HttpStatusCode.Ok,new IndexView());
=======
            var response = new ViewResponse(HttpStatusCode.Ok, new HomeIndexView());

            response.Cookies.Add(new HttpCookie("lang", "en"));

            return response;
>>>>>>> origin/master
        }
    }
}
