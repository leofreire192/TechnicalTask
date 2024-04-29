using FluentResults;
using System.Net;

namespace CustomersAPI
{
    public class CustomError : IError
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

        public List<IError> Reasons { get; } = new List<IError>();

        public Dictionary<string, object> Metadata { get; } = new Dictionary<string, object>();

        public CustomError(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public CustomError CausedBy(string message)
        {
            Reasons.Add(new Error(message));
            return this;
        }
    }

}
