using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Storage;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly IRpcCaller rpcCaller;

    public ProviderFactory(IRpcCaller? rpcCaller = null)
    {
        this.rpcCaller = rpcCaller ?? new RpcCaller();
    }

    public Erc20Token GetErc20Token(string address) => new(address, rpcCaller);

    public bool IsPoolIdWithinSupplyRange(BigInteger poolId) =>
        rpcCaller.GetTotalSupply(Environments.LockDealNftAddress) > poolId;

    public Provider Create(BigInteger poolId) =>
        Create(rpcCaller.GetMetadata(poolId));

    private Provider Create(string metadata) =>
        Create(new BasePoolInfo(metadata,this));

    private static Provider Create(BasePoolInfo basePoolInfo) =>
        Providers(basePoolInfo)[basePoolInfo.ProviderAddress]();

    public static Dictionary<string, Func<Provider>> Providers(BasePoolInfo basePoolInfo) => new()
    {
        { Environments.DealAddress, () => new DealProvider(basePoolInfo) },
        { Environments.LockDealAddress, () => new LockDealProvider(basePoolInfo) },
        { Environments.TimedDealAddress, () => new TimedDealProvider(basePoolInfo) },
        { Environments.RefundAddress, () => new RefundProvider(basePoolInfo) },
        { Environments.BundleAddress, () => new BundleProvider(basePoolInfo) },
        { Environments.CollateralAddress, () => new CollateralProvider(basePoolInfo) }
    };
}