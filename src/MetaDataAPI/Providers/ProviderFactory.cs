using System.Numerics;
using MetaDataAPI.RPC;
using MetaDataAPI.RPC.Models.PoolInfo;

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

    public Provider Create(BasePoolInfo poolInfo)
    {
        var objectToInstantiate = $"MetaDataAPI.Providers.{poolInfo.Provider}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, new List<BasePoolInfo> { poolInfo }, lockDealNFT.RpcUrl)!;
    }

    public T Create<T>(BigInteger poolId) where T : Provider => (T)Create(poolId);
    public T Create<T>(BasePoolInfo poolInfo) where T : Provider => (T)Create(poolInfo);

    private Dictionary<string, Func<Provider>> Providers(List<BasePoolInfo> poolInfo) => new()
    {
        { nameof(DealProvider), () => new DealProvider(new DealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        { nameof(LockDealProvider), () => new LockDealProvider(new LockDealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        { nameof(TimedDealProvider), () => new TimedDealProvider(new TimedDealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        { nameof(DelayVaultProvider), () => new DelayVaultProvider(new DealPoolInfo(poolInfo[0], lockDealNFT.RpcUrl)) },
        {
            nameof(CollateralProvider),
            () => new CollateralProvider(
                new CollateralPoolInfo(poolInfo[0], lockDealNFT.RpcUrl),
                poolInfo.Skip(0).Select(x => new DealPoolInfo(x, lockDealNFT.RpcUrl)).ToList()
            )
        },
        {
            nameof(RefundProvider),
            () => new RefundProvider(
                new RefundPoolInfo(poolInfo[0], lockDealNFT.RpcUrl))
        },
    };
}
