namespace WebServer.Server.Routing.Contracts
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Handlers;
    using Http.Contracts;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes { get; }

        ICollection<string> AnonymousPaths { get; }

        void Get(string route, Func<IHttpRequest, IHttpResponse> handler);

        void Post(string route, Func<IHttpRequest, IHttpResponse> handler);
<<<<<<< HEAD

=======
        
>>>>>>> b8e76d80beb0eff0ab4ae9ca15efe2b0b13a1fab
        void AddRoute(string route, HttpRequestMethod method, RequestHandler handler);
    }
}
