using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace ImageAPI.Utils;

public static class ResponseBuilder
{
    private const string missingIdMessage = "Missing or invalid id parameter";
    private const string generalErrorMessage = "Something went wrong";
    public static APIGatewayProxyResponse GetResponse(HttpStatusCode statusCode, string body) => new()
    {
        IsBase64Encoded = statusCode == HttpStatusCode.OK,
        StatusCode = (int)statusCode,
        Body = body,
        Headers = new Dictionary<string, string> { { "Content-Type", statusCode == HttpStatusCode.OK ? "image/png" : "text/plain" } }
    };
    public static APIGatewayProxyResponse WrongInput() => GetResponse(HttpStatusCode.BadRequest, missingIdMessage);
    public static APIGatewayProxyResponse ImageResponse(string base64Image) => GetResponse(HttpStatusCode.OK, base64Image);
    public static APIGatewayProxyResponse GeneralError() => GetResponse(HttpStatusCode.InternalServerError, generalErrorMessage);
}
