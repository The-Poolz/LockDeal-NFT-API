using System.Net;
using System.Numerics;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

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
            throw new InvalidOperationException("Invalid request. The 'id' parameter is missing.");
        }

        if (!BigInteger.TryParse(idParam, out var poolId))
        {
            throw new InvalidOperationException("Invalid request. The 'id' parameter is not a valid BigInteger.");
        }
      
        var provider = providerFactory.Create(poolId);

        if (poolId != provider.PoolInfo.PoolId)
        {
            throw new InvalidOperationException("Invalid response. Id from metadata needs to be the same as Id from request.");
        }

        Console.WriteLine(JToken.FromObject(provider));

        var jsonProvider = JsonConvert.SerializeObject(provider);
        var hash = DynamoDb.StringToSha256(jsonProvider);

        dynamoDb.PutItemAsync(hash, jsonProvider)
            .GetAwaiter()
            .GetResult();

        var response = provider.GetErc721Metadata();
        response.Image += hash;

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JObject.FromObject(response).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
