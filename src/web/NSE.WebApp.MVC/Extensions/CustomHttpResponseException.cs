

namespace NSE.WebApp.MVC.Extensions;

[System.Serializable]
public class CustomHttpResponseException : System.Exception
{
    public HttpStatusCode StatusCode { get; init; }

    public CustomHttpResponseException() { }
    public CustomHttpResponseException(string message) : base(message) { }
    public CustomHttpResponseException(string message, System.Exception inner) : base(message, inner) { }
    protected CustomHttpResponseException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    public CustomHttpResponseException(HttpStatusCode statusCode) => StatusCode = statusCode;
}