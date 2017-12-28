namespace WebServer.Server.Routing
{
    using System.Collections.Generic;
    using Common;
    using Contracts;
    using Handlers;

    public class RoutingContext:IRoutingContext
    {
     
        public RoutingContext(RequestHandler handler, IEnumerable<string> parameters)
        {
            MyValidator.ThrowIfNull(handler,nameof(handler));
            MyValidator.ThrowIfNull(parameters,nameof(parameters));


            this.Handler = handler;
            this.Parameters = parameters;
        }

        public IEnumerable<string> Parameters { get; private set; }

        public RequestHandler Handler { get; private set; }
    }
}
