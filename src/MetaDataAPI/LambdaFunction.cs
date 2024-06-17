using System.Numerics;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using MetaDataAPI.BlockchainManager;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly IChainManager chainManager;
    private readonly IProviderManager providerManager;

    public LambdaFunction()
    {
        // TODO: Implement chain manager which receive ChainInfo from DB.
        chainManager = new LocalChainManager();
        providerManager = new ProviderManager();
    }

    public LambdaFunction(IChainManager chainManager, IProviderManager providerManager)
    {
        this.chainManager = chainManager;
        this.providerManager = providerManager;
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        if (!request.QueryStringParameters.TryGetValue("chainId", out var stringChainId))
        {
            return new APIGatewayProxyResponse { Body = "Query string parameter 'chainId' is required." };
        }
        if (!BigInteger.TryParse(stringChainId, out var chainId))
        {
            return new APIGatewayProxyResponse { Body = $"Cannot parse '{chainId}' chain ID." };
        }

        if (!request.QueryStringParameters.TryGetValue("poolId", out var stringPoolId))
        {
            return new APIGatewayProxyResponse { Body = "Query string parameter 'poolId' is required." };
        }
        if (!BigInteger.TryParse(stringPoolId, out var poolId))
        {
            return new APIGatewayProxyResponse { Body = $"Cannot parse '{poolId}' pool ID." };
        }
        var chainInfo = chainManager.FetchChainInfo(chainId);

        var metadata = providerManager.Metadata(poolId, chainInfo);
        var serializedMetadata = JsonConvert.SerializeObject(metadata);

        Console.WriteLine(JToken.FromObject(metadata));
        return new APIGatewayProxyResponse
        {
            Body = serializedMetadata
        };
    }
}
