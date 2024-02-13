using System.Numerics;
using MetaDataAPI.Providers.Advanced;
using MetaDataAPI.Providers.DelayVault;
using MetaDataAPI.Providers.PoolsInfo;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.RPC;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly LockDealNFT lockDealNFT;

    public ProviderFactory(LockDealNFT? lockDealNFT = null)
    {
        this.lockDealNFT = lockDealNFT ?? new LockDealNFT();
    }

    public Provider Create(BigInteger poolId)
    {
        var basePoolInfo = lockDealNFT.GetFullData(poolId);
        var name = basePoolInfo[0].Name;
        var provider = Providers(basePoolInfo)[name]();
        return provider;
    }

    public T Create<T>(BigInteger poolId) where T : Provider => (T)Create(poolId);

    private Dictionary<string, Func<Provider>> Providers(List<PoolInfo> poolInfo) => new()
    {
        { nameof(DealProvider), () => new DealProvider(new DealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        { nameof(LockDealProvider), () => new LockDealProvider(new LockDealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        { nameof(TimedDealProvider), () => new TimedDealProvider(new TimedDealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        { nameof(DelayVaultProvider), () => new DelayVaultProvider(new DealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        {
            nameof(CollateralProvider),
            () => new CollateralProvider(
                new CollateralPoolInfo(poolInfo[0], lockDealNFT.RpcUrl), 
                poolInfo.Skip(1).Select(x => new DealPoolInfo(x, lockDealNFT.RpcUrl)).ToList()
            )
        },
        {
            nameof(RefundProvider),
            () => new RefundProvider(
                new RefundPoolInfo(poolInfo[0], lockDealNFT.RpcUrl),
                Create(poolInfo[0].PoolId + 1), 
                Create<CollateralProvider>(lockDealNFT.PoolIdToCollateralId(poolInfo[0].Provider, poolInfo[0].PoolId))
            )
        }
    };
}
