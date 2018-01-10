namespace WebServer.Server.Http
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Common;
    using Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, ICollection<HttpHeader>> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string,ICollection<HttpHeader>>();
        }
        
        public void Add(HttpHeader header)
        {
            MyValidator.ThrowIfNull(header,nameof(header));

            var headerKey = header.Key;

            if (!this.headers.ContainsKey(headerKey))
            {
                this.headers[headerKey] =new List<HttpHeader>();
            }

            this.headers[headerKey].Add(header);
        }

        public void Add(string key, string value)
        {
            MyValidator.ThrowIfNullOrEmpty(key, nameof(key));
            MyValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Add(new HttpHeader(key, value));
        }

        public bool ContainsKey(string key)
        {
           MyValidator.ThrowIfNull(key,nameof(key));

            return this.headers.ContainsKey(key);
        }

        public ICollection<HttpHeader> GetHeader(string key)
        {
            MyValidator.ThrowIfNull(key,nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw  new InvalidOperationException($"The given key {key} is not present in the headers collection.");
            }

            return this.headers[key];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var header in this.headers)
            {
                var headerKey = header.Key;

                foreach (var headerValue in header.Value)
                {
                    sb.AppendLine($"{headerKey}: {headerValue.Value}");
                }
            }

            return sb.ToString().Trim();
        }

        public IEnumerator<ICollection<HttpHeader>> GetEnumerator()
            => this.headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.headers.Values.GetEnumerator();

    }
}
