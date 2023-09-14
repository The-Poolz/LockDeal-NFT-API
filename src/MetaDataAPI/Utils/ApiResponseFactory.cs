using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Utils;

public static class ApiResponseFactory
{
    public static APIGatewayProxyResponse CreateResponse(string body, HttpStatusCode statusCode)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = body,
            Headers = new Dictionary<string, string>
            { { "Content-Type", statusCode == HttpStatusCode.OK? "application/json" : "text/plain" } }
        };
    }
}