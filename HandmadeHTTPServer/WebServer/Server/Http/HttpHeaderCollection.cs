namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }
        
        public void Add(HttpHeader header)
        {
            MyValidator.ThrowIfNull(header,nameof(header));

            this.headers[header.Key] = header;
        }

        public bool ContainsKey(string key)
        {
           MyValidator.ThrowIfNull(key,nameof(key));

            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            MyValidator.ThrowIfNull(key,nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw  new InvalidOperationException($"The given key {key} is not present in the headers collection.");
            }

            return this.headers[key];
        }

        public override string ToString()
       =>String.Join(Environment.NewLine,this.headers.Select(h=>h.Value.ToString()));
    }
}
