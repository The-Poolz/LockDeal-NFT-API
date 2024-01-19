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
        var objectToInstantiate = $"MetaDataAPI.Providers.{basePoolInfo[0].Provider}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, basePoolInfo, lockDealNFT.RpcUrl)!;
    }

    public Provider Create(BasePoolInfo poolInfo)
    {
        var objectToInstantiate = $"MetaDataAPI.Providers.{poolInfo.Provider}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, new List<BasePoolInfo> { poolInfo }, lockDealNFT.RpcUrl)!;
    }

    public T Create<T>(BigInteger poolId) where T : Provider => (T)Create(poolId);
    public T Create<T>(BasePoolInfo poolInfo) where T : Provider => (T)Create(poolInfo);
}