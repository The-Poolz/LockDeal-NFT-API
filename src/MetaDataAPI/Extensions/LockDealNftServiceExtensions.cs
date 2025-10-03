using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Extensions;

public static class LockDealNftServiceExtensions
{
    public static bool IsPoolIdInSupplyRange(this ILockDealNFTService lockDealNft, long poolId)
    {
        return lockDealNft.TotalSupplyQueryAsync()
            .GetAwaiter()
            .GetResult() > poolId;
    }

    public static BasePoolInfo[] FetchPoolInfo(this ILockDealNFTService lockDealNft, long poolId)
    {
        return lockDealNft.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }

    public static BasePoolInfo[] FetchPoolInfo(this IServiceProvider serviceProvider, long poolId)
    {
        return serviceProvider.GetRequiredService<ILockDealNFTService>()
            .GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }
}