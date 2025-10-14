using Amazon;
using Amazon.DynamoDBv2;
using MetaDataAPI.Extensions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using Amazon.DynamoDBv2.DataModel;
using EnvironmentManager.Extensions;
using Net.Cache.DynamoDb.ERC20.Models;
using MetaDataAPI.Services.ChainsInfo;

namespace MetaDataAPI.Services.Erc20;

public class Erc20Provider(ERC20CacheProvider provider) : IErc20Provider
{
    public Erc20Provider() : this(new ERC20CacheProvider(
        new ERC20StorageProvider(
            new DynamoDBContext(
                new AmazonDynamoDBClient(
                    new AmazonDynamoDBConfig
                    {
                        RegionEndpoint = RegionEndpoint.GetBySystemName(Environments.OVERRIDE_AWS_REGION.GetRequired())
                    }
                )
            )
        )
    ))
    {
    }

    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address)
    {
        return GetErc20Token(chainInfo.ChainId.ToRpcUrl(), chainInfo.ChainId, address);
    }

    public Erc20Token GetErc20Token(string rpcUrl, long chainId, EthereumAddress address)
    {
        var cache = provider.GetOrAdd(
            new GetCacheRequest(
                chainId: chainId,
                address,
                rpcUrl,
                false
            )
        );
        return new Erc20Token(cache);
    }
}