using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider Create(BigInteger poolId) => Create(RpcCaller.GetMetadata(poolId));
    public static IProvider Create(string metadata) => Create(new BasePoolInfo(metadata));
    public static IProvider Create(BasePoolInfo basePoolInfo) => Providers(basePoolInfo)[ProvidersAddresses(basePoolInfo.ProviderAddress)];
     public static Dictionary<ProviderName, IProvider> Providers(BasePoolInfo basePoolInfo) => new()
    {
        { ProviderName.Deal, new DealProvider(basePoolInfo) },
        { ProviderName.Lock, new LockProvider(basePoolInfo) },
        { ProviderName.Timed, new TimedProvider(basePoolInfo) },
        { ProviderName.Bundle, new BundleProvider(basePoolInfo) },
        { ProviderName.Refund, new RefundProvider(basePoolInfo) },
        { ProviderName.Collateral, new CollateralProvider(basePoolInfo) }
    };

    public static ProviderName ProvidersAddresses(string providerAddress)
    {
        var name = RpcCaller.GetName(providerAddress);
        return name switch
        {
            "DealProvider" => ProviderName.Deal,
            "LockProvider" => ProviderName.Lock,
            "TimedProvider" => ProviderName.Timed,
            "BundleProvider" => ProviderName.Bundle,
            "RefundProvider" => ProviderName.Refund,
            "Collateral" => ProviderName.Collateral,
            _ => throw new Exception($"Unknown provider name: {name}")
        };
    }
}