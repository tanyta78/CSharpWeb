﻿namespace WebServer.Server.Http
{

    using System.Collections.Generic;
    using Common;
    using Contracts;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> values;

        public HttpSession(string id)
        {
            MyValidator.ThrowIfNullOrEmpty(id, nameof(id));

            this.Id = id;
            this.values = new Dictionary<string, object>();
        }

        public string Id { get; private set; }

        public object Get(string key)
        {
            MyValidator.ThrowIfNull(key, nameof(key));

            if (!this.values.ContainsKey(key))
            {
                return null;
            }

            return this.values[key];
        }

        public T Get<T>(string key)
            => (T)this.Get(key);

        public void Add(string key, object value)
        {
            MyValidator.ThrowIfNullOrEmpty(key, nameof(key));
            MyValidator.ThrowIfNull(value, nameof(value));

            this.values[key] = value;

        }

        public void Clear() => this.values.Clear();

        public bool Contains(string key) => this.values.ContainsKey(key);
    }
}
