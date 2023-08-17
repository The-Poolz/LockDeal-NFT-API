using System.Numerics;
using MetaDataAPI.Storage;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider Create(string address, BigInteger poolId, string token) =>
        Create(ProvidersAddresses[address], poolId, token);

    public static IProvider Create(ProviderName name, BigInteger poolId, string token) =>
        Providers(poolId, token)[name];

    public static Dictionary<ProviderName, IProvider> Providers(BigInteger poolId, string token) => new()
    {
        { ProviderName.Deal, new DealProvider(token) },
        { ProviderName.Lock, new LockProvider(token) },
        { ProviderName.Timed, new TimedProvider(token) },
        { ProviderName.Bundle, new BundleProvider(poolId) },
        { ProviderName.Refund, new RefundProvider(poolId) },
        { ProviderName.Collateral, new CollateralProvider(poolId, token) }
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