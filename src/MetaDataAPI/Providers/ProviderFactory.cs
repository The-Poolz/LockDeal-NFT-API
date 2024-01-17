using System.Numerics;
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
        var objectToInstantiate = $"MetaDataAPI.Providers.{basePoolInfo[0].Provider}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, args: basePoolInfo)!;
    }
    public T Create<T>(BigInteger poolId) where T : Provider => (T)Create(poolId);
}