namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Common;
    using Contracts;
    using Enums;
    using Exceptions;

    public class HttpRequest : IHttpRequest
    {

        public HttpRequest(string requestText)
        {
            MyValidator.ThrowIfNullOrEmpty(requestText, nameof(requestText));

            this.FormData = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestText);
        }



        public IDictionary<string, string> FormData { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

        public string Path { get; private set; }

        public IDictionary<string, string> QueryParameters { get; private set; }

        public HttpRequestMethod Method { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public void AddUrlParameter(string key, string value)
        {
            MyValidator.ThrowIfNullOrEmpty(key, nameof(key));
            MyValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestText)
        {
            var requestLines = requestText.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            //{Method} {URL} HTTP/1.1
            var reqLine = requestLines.First().Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            if (reqLine.Length != 3
                || reqLine[2].ToLower() != "http/1.1")
            {
                BadRequestException.ThrowFromInvalidRequest();

            }

            this.Method = this.ParseMethod(reqLine.First());
            this.Url = reqLine[1];
            this.Path = this.ParsePath(this.Url);

            //{Headers} while empty line
            this.ParseHeaders(requestLines);
            this.ParseParameters();

            //{Form Data}
            this.ParseFormData(requestLines.Last());
        }  

        private void ParseFormData(string formDataLine)
        {
            if (this.Method == HttpRequestMethod.Get)
            {
                return;
            }
            // username=pesho&pass=133
           this.ParseQuery(formDataLine,this.QueryParameters);
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }
            
            var query = this.Url
                .Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Last();

            // /register?name=ivan
            this.ParseQuery(query,this.UrlParameters);
            
        }

        private void ParseHeaders(string[] requestLines)
        {
            var emptyLineIndex = Array.IndexOf(requestLines, String.Empty);
            for (int i = 1; i < emptyLineIndex; i++)
            {
                var headerParts = requestLines[i].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();

                }

                var headerKey = headerParts[0];
                var headerValue = headerParts[1].Trim();

                var header = new HttpHeader(headerKey, headerValue);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                BadRequestException.ThrowFromInvalidRequest();

            }
        }

        private string ParsePath(string url)
        {
            // / user/register/5?name=Pesho#gosho
            return url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private HttpRequestMethod ParseMethod(string method)
        {
            HttpRequestMethod parsedMethod;
            if (!Enum.TryParse<HttpRequestMethod>(method, true, out parsedMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            return parsedMethod;
        }

        private void ParseQuery(string query,IDictionary<string,string>dict)
        {
           if (!query.Contains('='))
            {
                return;
            }

            var queryPairs = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryPair in queryPairs)
            {
                var querykvp = queryPair.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                if (querykvp.Length != 2)
                {
                    return;
                }

                var queryKey = WebUtility.UrlDecode(querykvp[0]);
                var queryValue = WebUtility.UrlDecode(querykvp[1]);

               dict.Add(queryKey, queryValue);
            }
        }
    }
}

