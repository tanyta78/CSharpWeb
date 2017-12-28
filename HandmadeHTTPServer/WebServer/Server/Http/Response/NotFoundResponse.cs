namespace WebServer.Server.Http.Response
{
    using Contracts;
    using Enums;

    public class NotFoundResponse:HttpResponse
    {
        public NotFoundResponse()
        {
            this.StatusCode = HttpStatusCode.NotFound;
        }

      
    }
}
