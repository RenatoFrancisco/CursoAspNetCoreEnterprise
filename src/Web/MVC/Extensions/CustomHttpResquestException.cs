using System;
using System.Net;
using System.Runtime.Serialization;

namespace NSE.WebApp.MVC.Extensions
{
 public class CustomHttpResquestException : System.Exception
 {
     public HttpStatusCode StatusCode { get; }
     
     public CustomHttpResquestException() {}
     public CustomHttpResquestException(string message) : base(message) {}
     public CustomHttpResquestException(string message, Exception inner) : base(message, inner) {}
     public CustomHttpResquestException(SerializationInfo info, StreamingContext context) : base(info, context) {}
     public CustomHttpResquestException(HttpStatusCode statusCode) => StatusCode = statusCode;
 }
}