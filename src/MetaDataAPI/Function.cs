using System.Net;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Response;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class Function
{
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        var id = int.Parse(request.QueryStringParameters["id"]);

        var metadata = RpcCaller.GetMetadata(id);
        var parser = new MetadataParser(metadata);

        var basePoolInfo = new BasePoolInfo(
            provider: new Provider(parser.GetProviderAddress()),
            poolId: parser.GetPoolId(),
            owner: parser.GetOwnerAddress(),
            token: parser.GetTokenAddress(),
            parameters: parser.GetProviderParameters()
        );

        var responseBody = new Erc721Metadata(basePoolInfo);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JObject.FromObject(responseBody).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
