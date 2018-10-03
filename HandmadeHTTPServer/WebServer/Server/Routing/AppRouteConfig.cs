namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Contracts;
    using Enums;
    using Handlers;
    using Http.Contracts;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            this.AnonymousPaths = new List<string>();

            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>>();

            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.routes[method] = new Dictionary<string, RequestHandler>();
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes => this.routes;

        public ICollection<string> AnonymousPaths { get; private set; }

        public void Get(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
<<<<<<< HEAD
            this.AddRoute(route, HttpRequestMethod.Get, new RequestHandler(handler));
=======
            this.AddRoute(route,HttpRequestMethod.Get,new RequestHandler(handler));
>>>>>>> b8e76d80beb0eff0ab4ae9ca15efe2b0b13a1fab
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
<<<<<<< HEAD
            this.AddRoute(route, HttpRequestMethod.Post, new RequestHandler(handler));
        }

        public void AddRoute(string route, HttpRequestMethod method, RequestHandler handler)
        {
            this.routes[method].Add(route, handler);
=======
            this.AddRoute(route, HttpRequestMethod.Post,new RequestHandler(handler));
        }

        public void AddRoute(string route,HttpRequestMethod method, RequestHandler handler)
        {
          this.routes[method].Add(route,handler);
>>>>>>> b8e76d80beb0eff0ab4ae9ca15efe2b0b13a1fab
        }
    }
}
