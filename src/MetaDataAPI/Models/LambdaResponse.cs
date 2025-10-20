using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Models;

public abstract class LambdaResponse : APIGatewayHttpApiV2ProxyResponse
{
    protected LambdaResponse(string body, HttpStatusCode statusCode)
    {
        StatusCode = (int)statusCode;
        Body = body;
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", statusCode == HttpStatusCode.OK ? "application/json" : "text/plain" },
            { "Access-Control-Allow-Origin", "*" },
            { "Access-Control-Allow-Methods", "GET, OPTIONS" },
            { "Access-Control-Allow-Headers", "Content-Type" }
        };
    }
}