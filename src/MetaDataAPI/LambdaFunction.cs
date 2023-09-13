using System.Net;
using System.Numerics;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly ProviderFactory providerFactory;
    private readonly DynamoDb dynamoDb;

    public LambdaFunction() : this(new ProviderFactory(), new DynamoDb()) { }
    public LambdaFunction(ProviderFactory providerFactory, DynamoDb dynamoDb)
    {
        this.providerFactory = providerFactory;
        this.dynamoDb = dynamoDb;
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        if (!request.QueryStringParameters.TryGetValue("id", out var idParam))
        {
            return APIGatewayProxyResponses.GetErrorResponse("Invalid request. The 'id' parameter is missing.");
        }

        if (!BigInteger.TryParse(idParam, out var poolId))
        {
            return APIGatewayProxyResponses.GetErrorResponse("Invalid request. The 'id' parameter is not a valid BigInteger.");
        }

        var provider = providerFactory.Create(poolId);

        if (poolId != provider.PoolInfo.PoolId)
        {
            return APIGatewayProxyResponses.GetErrorResponse("Invalid response. Id from metadata needs to be the same as Id from request.");
        }

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = provider.GetJsonErc721Metadata(dynamoDb),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
