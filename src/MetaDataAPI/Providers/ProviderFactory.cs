using Nethereum.Web3;
using System.Numerics;
using EnvironmentManager.Extensions;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly LockDealNFTService lockDealNFTService;

    public ProviderFactory()
    {
        lockDealNFTService = new LockDealNFTService(new Web3(Environments.RPC_URL.Get()), Environments.LOCK_DEAL_NFT_ADDRESS.Get());
    }

    public ProviderFactory(LockDealNFTService lockDealNFTService)
    {
        this.lockDealNFTService = lockDealNFTService;
    }

    public bool IsPoolIdWithinSupplyRange(BigInteger poolId) => lockDealNFTService.TotalSupplyQueryAsync().GetAwaiter().GetResult() > poolId;

    public virtual Provider Create(BigInteger poolId) => Create(
        lockDealNFTService.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray()
        );

    public static Provider Create(BasePoolInfo[] basePoolInfo)
    {
        var objectToInstantiate = $"MetaDataAPI.Providers.{basePoolInfo.FirstOrDefault()!.Name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, new[] { basePoolInfo })!;
    }
}