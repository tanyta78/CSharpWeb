namespace WebServer.Server.Handlers
{
    using System;
    using Http.Contracts;

    public class GetHandler:RequestHandler
    {
        public GetHandler(Func<IHttpRequest, IHttpResponse> func) : base(func)
        {
        }
    }
}
