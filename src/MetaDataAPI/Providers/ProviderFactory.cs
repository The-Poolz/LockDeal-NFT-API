using System.Numerics;
using EnvironmentManager.Extensions;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly LockDealNFTService lockDealNFTService;

    public ProviderFactory(LockDealNFTService? lockDealNFTService = null)
    {
        this.lockDealNFTService = lockDealNFTService ??
            new LockDealNFTService(new Nethereum.Web3.Web3(Environments.RPC_URL.GetEnvironmentValue()), Environments.LOCK_DEAL_NFT_ADDRESS.GetEnvironmentValue());
    }
    public bool IsPoolIdWithinSupplyRange(BigInteger poolId) => lockDealNFTService.TotalSupplyQueryAsync().GetAwaiter().GetResult() > poolId;
    public Provider Create(BigInteger poolId) =>
        Create(lockDealNFTService.GetFullDataQueryAsync(poolId).GetAwaiter().GetResult().PoolInfo.ToArray());
    public static Provider Create(BasePoolInfo[] basePoolInfo)
    {
        var objectToInstantiate = $"MetaDataAPI.Providers.{basePoolInfo.FirstOrDefault()!.Name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, new[] { basePoolInfo })!;
    }
    public T Create<T>(BigInteger poolId) where T : Provider => (T)Create(poolId);
}