namespace WebServer.Server.Http.Response
{
    using Enums;
    using Exceptions;
    using Server.Contracts;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            this.ValidateStatusCode(statusCode);

            this.view = view;
            this.StatusCode = statusCode;
<<<<<<< HEAD
            
    
=======

            this.Headers.Add(HttpHeader.ContentType, "text/html");
>>>>>>> origin/master
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNumber = (int)statusCode;

            if (299 < statusCodeNumber && statusCodeNumber < 400)
            {
<<<<<<< HEAD
                throw new InvalidResponseException("View responses need a status code below 300 and above 400 (inclusive).");
=======
               throw new InvalidResponseException("View responses need a status code below 300 and above 400 (inclusive).");
>>>>>>> b8e76d80beb0eff0ab4ae9ca15efe2b0b13a1fab
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}
