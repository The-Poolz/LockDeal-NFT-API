using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Response;

public abstract class LambdaResponse : APIGatewayProxyResponse
{
    protected LambdaResponse(string body, HttpStatusCode statusCode)
    {
        StatusCode = (int)statusCode;
        Body = body;
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", statusCode == HttpStatusCode.OK ? "application/json" : "text/plain" }
        };
    }
}