using System.Numerics;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using MetaDataAPI.BlockchainManager;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Services.ChainsInfo;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly IServiceProvider serviceProvider;
    private readonly IChainManager chainManager;

    public LambdaFunction() // TODO: Implement chain manager which receive ChainInfo from DB.
        : this(new LocalChainManager())
    { }

    public LambdaFunction(IChainManager chainManager)
    {
        serviceProvider = DefaultServiceProvider.Instance;
        this.chainManager = chainManager;
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

        var poolsInfo = AbstractProvider.FetchPoolInfo(poolId, chainInfo);

        var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, serviceProvider);

        var metadata = provider.GetErc721Metadata();

        var serializedMetadata = JsonConvert.SerializeObject(metadata);

        Console.WriteLine(JToken.FromObject(metadata));

        return new APIGatewayProxyResponse
        {
            Body = serializedMetadata
        };
    }
}
