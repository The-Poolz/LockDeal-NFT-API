using System.Numerics;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using Net.Cache.DynamoDb.ERC20.Models;

namespace MetaDataAPI.Erc20Manager;

public class Erc20Provider : IErc20Provider
{
    private readonly ERC20CacheProvider provider;

    public Erc20Provider()
    {
        provider = new ERC20CacheProvider();
    }

    public Erc20Provider(ERC20CacheProvider provider)
    {
        this.provider = provider;
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