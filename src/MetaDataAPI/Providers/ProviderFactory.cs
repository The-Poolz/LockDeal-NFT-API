using System.Numerics;
using MetaDataAPI.Utils;
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

    public static Dictionary<ProviderName, IProvider> Providers(BigInteger poolId, string token)
    {
        var decimals = RpcCaller.GetDecimals(token);

        return new Dictionary<ProviderName, IProvider>
        {
            { ProviderName.Deal, new DealProvider(decimals) },
            { ProviderName.Lock, new LockProvider(decimals) },
            { ProviderName.Timed, new TimedProvider(decimals) },
            { ProviderName.Bundle, new BundleProvider(poolId) },
            { ProviderName.Refund, new RefundProvider(poolId) },
            { ProviderName.Collateral, new CollateralProvider(poolId, decimals) }
        };
    }

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