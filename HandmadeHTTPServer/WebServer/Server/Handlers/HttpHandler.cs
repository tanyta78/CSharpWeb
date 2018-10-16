namespace WebServer.Server.Handlers
{
    using System.Text.RegularExpressions;
    using Common;
    using Contracts;
    using Http.Contracts;
    using Http.Response;
    using Routing.Contracts;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            MyValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.serverRouteConfig = serverRouteConfig;
        }


        public IHttpResponse Handle(IHttpContext context)
        {
            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;
            var registeredRoutes = this.serverRouteConfig.Routes[requestMethod];

            foreach (var registeredRoute in registeredRoutes)
            {
                //^/users/(?<name>[a-b]+)$
                var routePattern = registeredRoute.Key;
                var routingContext = registeredRoute.Value;

                var routeRegex = new Regex(routePattern);
                var routeMatch = routeRegex.Match(requestPath);

                if (!routeMatch.Success)
                {

                    continue;
                }

                var parameters = routingContext.Parameters;

                foreach (var parameter in parameters)
                {
                    var parameterValue = routeMatch.Groups[parameter].Value;

                    context.Request.AddUrlParameter(parameter, parameterValue);
                }

                return routingContext.Handler.Handle(context);
            }

            return new NotFoundResponse();
        }
    }
}
