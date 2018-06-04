﻿namespace WebServer.Server.Http.Response
{
    using Common;
    using Enums;

    public class RedirectResponse:HttpResponse
    {
        public RedirectResponse(string redirectUrl)
        {
            MyValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            this.StatusCode = HttpStatusCode.Found;
            
            this.Headers.Add(new HttpHeader("Location", redirectUrl));
        }
    }
}
