﻿namespace WebServer.Server.Handlers
{
    using System;
    using Http.Contracts;

    public  class PostHandler:RequestHandler
    {
        public PostHandler(Func<IHttpRequest, IHttpResponse> func) : base(func)
        {
        }
    }
}
