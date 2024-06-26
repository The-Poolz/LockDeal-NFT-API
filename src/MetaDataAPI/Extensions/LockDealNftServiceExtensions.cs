using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Extensions;

public static class LockDealNftServiceExtensions
{
    public static bool IsPoolIdInSupplyRange(this ILockDealNFTService lockDealNft, BigInteger poolId)
    {
        return lockDealNft.TotalSupplyQueryAsync()
            .GetAwaiter()
            .GetResult() > poolId;
    }

    public static BasePoolInfo[] FetchPoolInfo(this ILockDealNFTService lockDealNft, BigInteger poolId)
    {
        return lockDealNft.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }

    public static BasePoolInfo[] FetchPoolInfo(this IServiceProvider serviceProvider, BigInteger poolId)
    {
        return serviceProvider.GetRequiredService<ILockDealNFTService>()
            .GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }
}