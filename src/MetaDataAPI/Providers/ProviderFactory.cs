using System.Numerics;
using MetaDataAPI.Storage;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider Create(string address, BigInteger poolId, byte decimals, BigInteger[] values) =>
         Create(ProvidersAddresses[address], poolId, decimals, values);

    public static IProvider Create(ProviderName name, BigInteger poolId, byte decimals, BigInteger[] values) =>
        Providers(poolId, decimals, values)[name]();

    public static Dictionary<ProviderName, Func<IProvider>> Providers(BigInteger poolId, byte decimals, BigInteger[] values) => new()
    {
        { ProviderName.Deal, () => new DealProvider(decimals, values) },
        { ProviderName.Lock, () => new LockProvider(decimals, values) },
        { ProviderName.Timed, () => new TimedProvider(decimals, values) },
        { ProviderName.Bundle, () => new BundleProvider(poolId, decimals, values) },
        { ProviderName.Refund, () => new RefundProvider(poolId, decimals, values) },
        { ProviderName.Collateral, () => new CollateralProvider(poolId, decimals, values) }
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