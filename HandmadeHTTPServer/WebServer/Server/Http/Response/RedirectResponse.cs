namespace WebServer.Server.Http.Response
{
    using Common;
    using Enums;

    public class RedirectResponse:HttpResponse
    {
        public RedirectResponse(string redirectUrl)
        {
            MyValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            this.StatusCode = HttpStatusCode.Found;
<<<<<<< HEAD
            
            this.Headers.Add(new HttpHeader("Location", redirectUrl));
=======
            this.Headers.Add(HttpHeader.Location,redirectUrl);
>>>>>>> origin/master
        }
    }
}
