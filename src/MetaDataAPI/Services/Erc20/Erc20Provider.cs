using MetaDataAPI.Extensions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using MetaDataAPI.Services.Http;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using Poolz.Finance.CSharp.Polly.Extensions;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace MetaDataAPI.Services.Erc20;

public class Erc20Provider(
    IErc20CacheService erc20Cache,
    IWeb3Factory web3Factory,
    IRetryExecutor retry
) : IErc20Provider
{
    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address)
    {
        return GetErc20Token(chainInfo.ChainId.ToRpcUrl(), chainInfo.ChainId, address);
    }

    public Erc20Token GetErc20Token(string rpcUrl, long chainId, EthereumAddress address)
    {
        var cache = retry.Execute(_ =>
        {
            return erc20Cache.GetOrAddAsync(
                new HashKey(chainId, address),
                () => Task.FromResult(web3Factory.Create(rpcUrl)),
                () => Task.FromResult(new EthereumAddress(Env.MULTI_CALL_V3_ADDRESS.GetRequired()))
            ).GetAwaiter().GetResult();
        });
        return new Erc20Token(cache);
    }
}