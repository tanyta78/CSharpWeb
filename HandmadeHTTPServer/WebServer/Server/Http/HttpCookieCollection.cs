namespace WebServer.Server.Http
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using Contracts;

    public class HttpCookieCollection:IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies=new Dictionary<string, HttpCookie>();
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return this.cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(HttpCookie cookie)
        {
            MyValidator.ThrowIfNull(cookie,nameof(cookie));

            this.cookies[cookie.Key] = cookie;
        }

        public void Add(string key, string value)
        {
            MyValidator.ThrowIfNullOrEmpty(key, nameof(key));
            MyValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Add(new HttpCookie(key, value));
        }

        public bool ContainsKey(string key)
        {
            MyValidator.ThrowIfNull(key, nameof(key));

            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            MyValidator.ThrowIfNull(key, nameof(key));

            if (!this.cookies.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key {key} is not present in the cookies collection.");
            }

            return this.cookies[key];
        }
    }
}