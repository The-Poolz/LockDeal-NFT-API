﻿using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Services.Strapi;
using MetaDataAPI.Services.ChainsInfo;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;

namespace MetaDataAPI;

public static class DefaultServiceProvider
{
    private static readonly Lazy<IServiceProvider> LazyInstance = new(() =>
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IStrapiClient, StrapiClient>(_ => new StrapiClient());
        serviceCollection.AddSingleton<IChainManager, StrapiChainManager>(x => new StrapiChainManager(x.GetRequiredService<IStrapiClient>()));
        serviceCollection.AddSingleton<IErc20Provider, Erc20Provider>(_ => new Erc20Provider());
        serviceCollection.AddSingleton<ILockDealNFTService, LockDealNFTService>(_ => new LockDealNFTService());

        return serviceCollection.BuildServiceProvider();
    });

    public static IServiceProvider Instance => LazyInstance.Value;
}