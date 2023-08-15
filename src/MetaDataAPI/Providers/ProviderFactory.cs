using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Providers;

public static class ProviderFactory
{
    public static IProvider Create(string address) =>
        Create(ProvidersAddresses[address]);

    public static IProvider Create(ProviderName name) =>
        Providers[name];

    public static Dictionary<ProviderName, IProvider> Providers => new()
    {
        { ProviderName.Deal, new DealProvider() },
        { ProviderName.Lock, new LockProvider() },
        { ProviderName.Timed, new TimedProvider() },
        { ProviderName.Bundle, new BundleProvider() },
        //{ ProviderName.Refund, new RefundProvider() },
        //{ ProviderName.Collateral, new CollateralProvider() }
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