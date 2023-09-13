using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Utils;

public static class ApiGatewayProxyResponses
{
    public static APIGatewayProxyResponse GetErrorResponse(string errorMessage)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Body = errorMessage,
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };
    }
}