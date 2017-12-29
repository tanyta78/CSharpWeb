﻿namespace WebServer.Server.Http
{
    using System;
    using Common;

    public class HttpHeader
    {
        public HttpHeader(string key, string value)
        {
            MyValidator.ThrowIfNullOrEmpty(key,nameof(key));
            MyValidator.ThrowIfNullOrEmpty(value,nameof(value));

            this.Key = key;
            this.Value = value;
        }
        
        public string Key { get; private set; }

        public string Value { get; private set; }

        public override string ToString() => this.Key + ": " + this.Value;
    }
}