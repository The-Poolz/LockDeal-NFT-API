using System.Numerics;
using MetaDataAPI.Extensions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using Net.Cache.DynamoDb.ERC20.Models;
using MetaDataAPI.Services.ChainsInfo;

namespace MetaDataAPI.Services.Erc20;

public class Erc20Provider(ERC20CacheProvider provider) : IErc20Provider
{
    public Erc20Provider() : this(new ERC20CacheProvider())
    {
    }

    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address)
    {
        return GetErc20Token(chainInfo.ChainId.ToRpcUrl(), chainInfo.ChainId, address);
    }

    public Erc20Token GetErc20Token(string rpcUrl, BigInteger chainId, EthereumAddress address)
    {
        var cache = provider.GetOrAdd(
            new GetCacheRequest(
                chainId: (long)chainId, //TODO: Check for potential overflow or data loss when upcasting BigInteger to Int64
                address,
                rpcUrl,
                false
            )
        );
        return new Erc20Token(cache);
    }
}