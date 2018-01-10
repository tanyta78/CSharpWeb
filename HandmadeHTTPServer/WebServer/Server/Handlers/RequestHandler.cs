namespace WebServer.Server.Handlers
{
    using System;
    using Common;
    using Contracts;
    using Http;
    using Http.Contracts;
   
    public abstract class RequestHandler:IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> func)
        {
            MyValidator.ThrowIfNull(func,nameof(func));
            
            this.handlingFunc = func;
        }
        
        public IHttpResponse Handle(IHttpContext httpContext)
        {
            var response = this.handlingFunc(httpContext.Request);

            if (!response.Headers.ContainsKey(HttpHeader.ContentType))
            {
                response.Headers.Add(HttpHeader.ContentType, "text/plain");
            }

            foreach (var cookie in response.Cookies)
            {
                response.Headers.Add(HttpHeader.SetCookie, cookie.ToString());
            }

            return response;
        }
    }
}
