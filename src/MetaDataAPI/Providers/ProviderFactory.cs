using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider Create(string address, BigInteger poolId) =>
        Create(ProvidersAddresses[address], poolId);

    public static IProvider Create(ProviderName name, BigInteger poolId) =>
        Providers(poolId)[name];

    public static Dictionary<ProviderName, IProvider> Providers(BigInteger poolId) => new()
    {
        { ProviderName.Deal, new DealProvider() },
        { ProviderName.Lock, new LockProvider() },
        { ProviderName.Timed, new TimedProvider() },
        { ProviderName.Bundle, new BundleProvider(poolId) },
        { ProviderName.Refund, new RefundProvider(poolId) },
        { ProviderName.Collateral, new CollateralProvider(poolId) }
    };

    public static Dictionary<string, ProviderName> ProvidersAddresses => new()
    {
        { Environments.DealAddress, ProviderName.Deal },
        { Environments.LockAddress, ProviderName.Lock },
        { Environments.TimedAddress, ProviderName.Timed },
        { Environments.RefundAddress, ProviderName.Refund },
        { Environments.BundleAddress, ProviderName.Bundle },
        { Environments.CollateralAddress, ProviderName.Collateral }
    };
}