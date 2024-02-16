using System.Numerics;
using MetaDataAPI.Storage;
using poolz.finance.csharp.LockDealNFT;
using poolz.finance.csharp.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class ProviderFactory(LockDealNFTService? _lockDealNFTService = null)
{
    private readonly LockDealNFTService lockDealNFTService = _lockDealNFTService ??
            new LockDealNFTService(new Nethereum.Web3.Web3(Environments.RpcUrl), Environments.LockDealNftAddress);
    public bool IsPoolIdWithinSupplyRange(BigInteger poolId) => lockDealNFTService.TotalSupplyQueryAsync().GetAwaiter().GetResult() > poolId;
    public Provider Create(BigInteger poolId) =>
        Create([.. lockDealNFTService.GetFullDataQueryAsync(poolId).GetAwaiter().GetResult().PoolInfo]);
    public static Provider Create(BasePoolInfo[] basePoolInfo)
        => (Provider)Activator.CreateInstance(ProviderType(basePoolInfo.FirstOrDefault()!), new[] { basePoolInfo })!;
    internal static Type ProviderType(BasePoolInfo basePoolInfo) => Type.GetType(GetClassName(basePoolInfo)) ??
        throw new NullReferenceException("basePoolInfo Must have value.");
    internal static string GetClassName(string ProviderName) => $"MetaDataAPI.Providers.{ProviderName}, MetaDataAPI";
    internal static string GetClassName(BasePoolInfo basePoolInfo) => GetClassName(basePoolInfo.Name);
}