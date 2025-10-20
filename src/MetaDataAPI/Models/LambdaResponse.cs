using System.Net;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Models;

public abstract class LambdaResponse : ApplicationLoadBalancerResponse
{
    protected LambdaResponse(string body, HttpStatusCode statusCode)
    {
        StatusCode = (int)statusCode;
        StatusDescription = statusCode.ToString();
        Body = body;
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", statusCode == HttpStatusCode.OK ? "application/json" : "text/plain" }
        };
    }
}