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
    public static IProvider Create(BasePoolInfo basePoolInfo) => Providers(basePoolInfo)[ProvidersAddresses(basePoolInfo.ProviderAddress)]();
    public static Dictionary<ProviderName, Func<IProvider>> Providers(BasePoolInfo basePoolInfo) => new()
    {
        { ProviderName.DealProvider,() => new DealProvider(basePoolInfo) },
        { ProviderName.LockDealProvider,() => new LockProvider(basePoolInfo) },
        { ProviderName.TimedDealProvider,() => new TimedProvider(basePoolInfo) },
        { ProviderName.BundleProvider,() => new BundleProvider(basePoolInfo) },
        { ProviderName.RefundProvider,() => new RefundProvider(basePoolInfo) },
        { ProviderName.CollateralProvider,() => new CollateralProvider(basePoolInfo) }
    };

    public static ProviderName ProvidersAddresses(string providerAddress) =>
        Enum.Parse<ProviderName>(RpcCaller.GetName(providerAddress));
}