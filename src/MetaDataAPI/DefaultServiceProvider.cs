using TLY.ShortUrl;
using MetaDataAPI.Services.Erc20;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;

namespace MetaDataAPI;

public static class DefaultServiceProvider
{
    private static readonly Lazy<IServiceProvider> LazyInstance = new(() =>
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IChainManager, DbChainManager>(_ => new DbChainManager());
        serviceCollection.AddSingleton<IErc20Provider, Erc20Provider>(_ => new Erc20Provider());
        serviceCollection.AddSingleton<ITlyContext, TlyContext>(_ => new TlyContext(Environments.TLY_API_KEY.Get()));
        serviceCollection.AddSingleton<ILockDealNFTService, LockDealNFTService>(_ => new LockDealNFTService());

        return serviceCollection.BuildServiceProvider();
    });

    public static IServiceProvider Instance => LazyInstance.Value;
}