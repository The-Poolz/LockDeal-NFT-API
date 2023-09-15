using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.Utils;

public static class ResponseBuilder
{
    private const string MissingHashMessage = "Missing 'hash' parameter.";
    private const string ItemNotFoundMessage = "Item by provided 'hash' not found.";
    private const string GeneralErrorMessage = "Something went wrong.";
    public static APIGatewayProxyResponse GetResponse(HttpStatusCode statusCode, string body) => new()
    {
        IsBase64Encoded = statusCode == HttpStatusCode.OK,
        StatusCode = (int)statusCode,
        Body = body,
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", statusCode == HttpStatusCode.OK ? "image/png" : "text/plain" }
        }
    };
    public static APIGatewayProxyResponse WrongInput() => GetResponse(HttpStatusCode.BadRequest, MissingHashMessage);
    public static APIGatewayProxyResponse WrongHash() => GetResponse(HttpStatusCode.NotFound, ItemNotFoundMessage);
    public static APIGatewayProxyResponse ImageResponse(string base64Image) => GetResponse(HttpStatusCode.OK, base64Image);
    public static APIGatewayProxyResponse GeneralError() => GetResponse(HttpStatusCode.InternalServerError, GeneralErrorMessage);
}
