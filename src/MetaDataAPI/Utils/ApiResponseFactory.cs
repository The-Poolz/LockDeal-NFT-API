using System.Net;
using System.Reflection;
using MetaDataAPI.Attributes;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Utils;

public static class ApiResponseFactory
{
    public static APIGatewayProxyResponse CreateResponse(string body, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = body,
            Headers = new Dictionary<string, string>
            { { "Content-Type", statusCode == HttpStatusCode.OK ? "application/json" : "text/plain" } }
        };
    }

    public static APIGatewayProxyResponse Create(this ErrorResponses errorMessage)
    {
        var attribute = errorMessage.GetType().GetCustomAttribute<ErrorMessageAttribute>();
        return attribute == null
            ? CreateResponse("Failed to create response.", HttpStatusCode.InternalServerError)
            : CreateResponse(attribute.Description, attribute.StatusCode);
    }
}