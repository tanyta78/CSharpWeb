﻿namespace WebServer.Server.Http.Response
{
    using System.Text;
    using Contracts;
    using Enums;


    public abstract class HttpResponse:IHttpResponse
    {
        private string statusCodeMessage => this.StatusCode.ToString();

        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }

        public IHttpHeaderCollection Headers { get; }
        public IHttpCookieCollection Cookies { get; }

        public HttpStatusCode StatusCode { get; protected set; }


        public override string ToString()
        {
            var response = new StringBuilder();

            var statusCode = (int)this.StatusCode;

            response.AppendLine($"HTTP/1.1 {statusCode} {this.statusCodeMessage}");

            response.AppendLine(this.Headers.ToString());
          
            return response.ToString();
        }
    }
}
