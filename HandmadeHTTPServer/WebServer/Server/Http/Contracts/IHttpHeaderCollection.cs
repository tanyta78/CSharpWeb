﻿namespace WebServer.Server.Http.Contracts
{
    using System.Collections.Generic;

    public interface IHttpHeaderCollection:IEnumerable<ICollection<HttpHeader>>
   {
       void Add(HttpHeader header);

       void Add(string key, string value);

       bool ContainsKey(string key);

       ICollection<HttpHeader> GetHeader(string key);
   }
}
