namespace WebServer.Application.Controllers
{
    using System;
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
            var response = new ViewResponse(HttpStatusCode.Ok, new IndexView());
=======
<<<<<<< HEAD
            return new ViewResponse(HttpStatusCode.Ok,new IndexView());
=======
            var response = new ViewResponse(HttpStatusCode.Ok, new HomeIndexView());
>>>>>>> b8e76d80beb0eff0ab4ae9ca15efe2b0b13a1fab

            response.Cookies.Add(new HttpCookie("lang", "en"));

            return response;
>>>>>>> origin/master
        }

        //GET/testsession
        public IHttpResponse SessionTest(IHttpRequest req)
        {
            var session = req.Session;

            const string sessionDateKey = "Saved_Date";

            if (session.Get(sessionDateKey) == null)
            {

                session.Add(sessionDateKey, DateTime.UtcNow);
            }

            return new ViewResponse(HttpStatusCode.Ok, new SessionTestView(session.Get<DateTime>(sessionDateKey)));
        }
    }
}
