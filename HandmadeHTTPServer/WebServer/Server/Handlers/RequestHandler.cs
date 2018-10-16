namespace WebServer.Server.Handlers
{
    using System;
    using Common;
    using Contracts;
    using Http;
    using Http.Contracts;
<<<<<<< HEAD

    public class RequestHandler : IRequestHandler
=======
   
    public class RequestHandler:IRequestHandler
>>>>>>> b8e76d80beb0eff0ab4ae9ca15efe2b0b13a1fab
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        public RequestHandler(Func<IHttpRequest, IHttpResponse> func)
        {
            MyValidator.ThrowIfNull(func, nameof(func));

            this.handlingFunc = func;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            string sessionIdToSend = null;

            if (!httpContext.Request.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var sessionId = Guid.NewGuid().ToString();

                httpContext.Request.Session = SessionStore.Get(sessionId);

                sessionIdToSend = sessionId;
            }

            var response = this.handlingFunc(httpContext.Request);
<<<<<<< HEAD
            
            response.Headers.Add(new HttpHeader("Content-Type", "text/html"));
=======

            if (sessionIdToSend != null)
            {
                response.Headers.Add(HttpHeader.SetCookie, $"{SessionStore.SessionCookieKey}={sessionIdToSend}; HttpOnly; path=/");
            }

            if (!response.Headers.ContainsKey(HttpHeader.ContentType))
            {
                response.Headers.Add(HttpHeader.ContentType, "text/plain");
            }

            foreach (var cookie in response.Cookies)
            {
                response.Headers.Add(HttpHeader.SetCookie, cookie.ToString());
            }
>>>>>>> origin/master

            return response;
        }
    }
}
