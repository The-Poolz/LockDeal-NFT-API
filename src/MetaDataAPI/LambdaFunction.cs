using System.Numerics;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using MetaDataAPI.BlockchainManager;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.BlockchainManager.Models;
using Nethereum.Web3;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using poolz.finance.csharp.contracts.LockDealNFT;
using MetaDataAPI.Erc20Manager;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly IChainManager chainManager;
    private readonly IErc20Provider erc20Provider;

    public LambdaFunction()
    {
        // TODO: Implement chain manager which receive ChainInfo from DB.
        chainManager = new LocalChainManager();
        erc20Provider = new Erc20Provider();
    }

    public LambdaFunction(IChainManager chainManager, IErc20Provider erc20Provider)
    {
        this.chainManager = chainManager;
        this.erc20Provider = erc20Provider;
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

        var poolsInfo = FetchPoolInfo(poolId, chainInfo);
        var poolInfo = poolsInfo.FirstOrDefault()!;

        var type = Type.GetType($"MetaDataAPI.Providers.{poolInfo.Name}, MetaDataAPI")
            ?? throw new InvalidOperationException($"Cannot found '{poolInfo.Name}' type. Please check if this Provider implemented.");
        var provider = (AbstractProvider)Activator.CreateInstance(type, poolsInfo, chainInfo, erc20Provider)!;

        var metadata = provider.GetErc721Metadata();
        var serializedMetadata = JsonConvert.SerializeObject(metadata);

        Console.WriteLine(JToken.FromObject(metadata));
        return new APIGatewayProxyResponse
        {
            Body = serializedMetadata
        };
    }

    public BasePoolInfo[] FetchPoolInfo(BigInteger poolId, ChainInfo chainInfo)
    {
        return FetchPoolInfo(poolId, new LockDealNFTService(new Web3(chainInfo.RpcUrl), chainInfo.LockDealNFT));
    }

    public BasePoolInfo[] FetchPoolInfo(BigInteger poolId, LockDealNFTService lockDealNFTService)
    {
        return lockDealNFTService.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }
}
