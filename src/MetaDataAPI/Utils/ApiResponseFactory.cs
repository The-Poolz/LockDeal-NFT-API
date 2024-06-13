using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Utils;

public static class ApiResponseFactory
{
    public static APIGatewayProxyResponse InvalidId()
    {
        return CreateResponse("The 'id' parameter is not a valid BigInteger.", HttpStatusCode.BadRequest);
    }

    public static APIGatewayProxyResponse MissingId()
    {
        return CreateResponse("The 'id' parameter is missing.", HttpStatusCode.BadRequest);
    }

    public static APIGatewayProxyResponse PoolIdNotInRange()
    {
        return CreateResponse("The 'id' need to be less then total supply", HttpStatusCode.UnprocessableEntity);
    }

    public static APIGatewayProxyResponse InvalidResponse()
    {
        return CreateResponse("Id from metadata needs to be the same as Id from request.", HttpStatusCode.UnprocessableEntity);
    }

    public static APIGatewayProxyResponse FailedToCreateProvider()
    {
        return CreateResponse("Failed to create provider.", HttpStatusCode.UnprocessableEntity);
    }

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