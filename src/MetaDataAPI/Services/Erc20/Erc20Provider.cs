using MetaDataAPI.Extensions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace MetaDataAPI.Services.Erc20;

public class Erc20Provider(IErc20CacheService erc20Cache) : IErc20Provider
{
    public Erc20Provider() : this(new Erc20CacheService()) { }

    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address)
    {
        return GetErc20Token(chainInfo.ChainId.ToRpcUrl(), chainInfo.ChainId, address);
    }

    public Erc20Token GetErc20Token(string rpcUrl, long chainId, EthereumAddress address)
    {
        var cache = erc20Cache.GetOrAddAsync(
            new HashKey(chainId, address),
            () => Task.FromResult(rpcUrl),
            () => Task.FromResult(new EthereumAddress(Env.MULTI_CALL_V3_ADDRESS.GetRequired()))
        ).GetAwaiter().GetResult();
        return new Erc20Token(cache);
    }
}