using System.Net;

namespace MetaDataAPI.Response;

public class ErrorMessageAttribute : Attribute
{
    public HttpStatusCode StatusCode { get; set; }
    public string Description { get; set; }

    public ErrorMessageAttribute(HttpStatusCode statusCode, string description)
    {
        StatusCode = statusCode;
        Description = description;
    }
}