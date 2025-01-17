﻿namespace WebServer.Server.Http.Contracts
{
    using System.Collections.Generic;

    public interface IHttpCookieCollection:IEnumerable<HttpCookie>
    {
        void Add(HttpCookie cookie);

        void Add(string key, string value);

        bool ContainsKey(string key);

        HttpCookie GetCookie(string key);
    }
}