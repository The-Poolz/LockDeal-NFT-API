using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using Net.Cache.DynamoDb.ERC20.Models;
using MetaDataAPI.Services.ChainsInfo;

namespace MetaDataAPI.Services.Erc20;

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

    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address)
    {
        return GetErc20Token(chainInfo.RpcUrl, chainInfo.ChainId, address);
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