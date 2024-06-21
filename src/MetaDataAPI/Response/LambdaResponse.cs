using System.Net;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Response;

public class LambdaResponse : APIGatewayProxyResponse
{
    public LambdaResponse(ErrorResponse error)
        : this(error.ErrorMessage, error.StatusCode)
    { }

    public LambdaResponse(Erc721Metadata metadata)
        : this(JsonConvert.SerializeObject(metadata), HttpStatusCode.OK)
    { }

    private LambdaResponse(string body, HttpStatusCode statusCode)
    {
        StatusCode = (int)statusCode;
        Body = body;
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", statusCode == HttpStatusCode.OK ? "application/json" : "text/plain" }
        };
    }
}