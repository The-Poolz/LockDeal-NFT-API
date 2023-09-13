using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Providers;
using System.Net;

namespace MetaDataAPI.Utils;

public static class APIGatewayProxyResponses
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