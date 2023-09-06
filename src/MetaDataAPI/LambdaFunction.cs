using System.Net;
using System.Numerics;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public static class LambdaFunction
{
    public static APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        var poolId = BigInteger.Parse(request.QueryStringParameters["id"]);
        var metadata = RpcCaller.GetMetadata(poolId);
        var basePoolInfo = new BasePoolInfo(metadata);
        var responseBody = new Erc721Metadata(basePoolInfo);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JObject.FromObject(responseBody).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
