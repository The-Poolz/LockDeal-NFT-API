using Nethereum.Web3;
using System.Numerics;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using MetaDataAPI.Providers;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.BlockchainManager;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.BlockchainManager.Models;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

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

        var fullPoolInfo = FetchPoolInfo(poolId, chainInfo);
        var poolInfo = fullPoolInfo.FirstOrDefault()!;

        var erc20 = erc20Provider.GetErc20Token(chainInfo.RpcUrl, chainId, poolInfo.Token);

        var objectToInstantiate = $"MetaDataAPI.Providers.{poolInfo.Name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        var provider = (Provider)Activator.CreateInstance(objectType!, new object[] { poolInfo, erc20 })!;

        var metadata = provider.Metadata(poolId);
        var serializedMetadata = JsonConvert.SerializeObject(metadata);

        Console.WriteLine(serializedMetadata);
        return new APIGatewayProxyResponse
        {
            Body = serializedMetadata
        };
    }

    public IEnumerable<BasePoolInfo> FetchPoolInfo(BigInteger poolId, ChainInfo chainInfo)
    {
        return FetchPoolInfo(poolId, new LockDealNFTService(new Web3(chainInfo.RpcUrl), chainInfo.LockDealNFT));
    }

    public IEnumerable<BasePoolInfo> FetchPoolInfo(BigInteger poolId, LockDealNFTService lockDealNFTService)
    {
        return lockDealNFTService.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }
}
