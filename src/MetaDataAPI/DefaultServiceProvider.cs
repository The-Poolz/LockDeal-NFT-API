using TLY.ShortUrl;
using MetaDataAPI.Services.Erc20;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using Microsoft.Extensions.DependencyInjection;

namespace MetaDataAPI;

public static class DefaultServiceProvider
{
    private static readonly Lazy<IServiceProvider> LazyInstance = new(() =>
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IChainManager, LocalChainManager>(_ => new LocalChainManager());
        serviceCollection.AddSingleton<IErc20Provider, Erc20Provider>(_ => new Erc20Provider());
        serviceCollection.AddSingleton<ITlyContext, TlyContext>(_ => new TlyContext(Environments.TLY_API_KEY.Get()));

        return serviceCollection.BuildServiceProvider();
    });

    public static IServiceProvider Instance => LazyInstance.Value;
}