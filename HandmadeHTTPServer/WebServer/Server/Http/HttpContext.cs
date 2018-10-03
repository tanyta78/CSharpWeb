namespace WebServer.Server.Http
{
    using Common;
    using Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(IHttpRequest request)
        {
            MyValidator.ThrowIfNull(request,nameof(request));

            this.request = request;
        }

        public IHttpRequest Request => this.request;
    }
}
