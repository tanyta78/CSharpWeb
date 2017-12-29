namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Contracts;
    using Enums;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.routes = new Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>>();

            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeRouteConfig(appRouteConfig);
        }

        private void InitializeRouteConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                var routesWithHandlers = registeredRoute.Value;
                var requestMethod = registeredRoute.Key;


                foreach (var routeWithHandler in routesWithHandlers)
                {
                    var route = routeWithHandler.Key;
                    var handler = routeWithHandler.Value;
                    var parameters = new List<string>();
                    var parsedRouteRegex = this.ParseRoute(route, parameters);
                    var routingContext = new RoutingContext(handler, parameters);
                    this.routes[requestMethod].Add(parsedRouteRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string route, List<string> parameters)
        {
            var parsedRegex = new StringBuilder();
            parsedRegex.Append('^');

            if (route == "/")
            {
                parsedRegex.Append("/$");
                return parsedRegex.ToString();
            }

            var tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(parameters, tokens, parsedRegex);

            return parsedRegex.ToString();
        }

        private void ParseTokens(List<string> parameters, string[] tokens, StringBuilder parsedRegex)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                var end = i == tokens.Length - 1 ? "$" : "/";
                var currentToken = tokens[i];

                if (!currentToken.StartsWith("(") && !currentToken.EndsWith(")"))
                {
                    parsedRegex.Append($"{currentToken}{end}");
                    continue;
                }

                var pattern = "<\\w+>";
                var parameterRegex = new Regex(pattern);
                var parameterMatch = parameterRegex.Match(currentToken);

                if (!parameterMatch.Success)
                {
                    throw new InvalidOperationException($"Route parameter in '{currentToken}'is not valid!");
                }

                var match = parameterMatch.Value;
                var paramName = match.Substring(1, match.Length - 2);
                parameters.Add(paramName);
                parsedRegex.Append($"{currentToken.Substring(1, currentToken.Length - 2)}{end}");
            }
        }


        public Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes => this.routes;
    }
}
