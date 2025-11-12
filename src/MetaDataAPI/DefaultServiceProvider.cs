using System.Reflection;
using Net.Cache.DynamoDb.ERC20;
using MetaDataAPI.Services.Http;
using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Services.Strapi;
using GraphQL.Client.Abstractions;
using MetaDataAPI.Services.ChainsInfo;
using Poolz.Finance.CSharp.Polly.Extensions;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;
using MediatR.Extensions.FluentValidation.AspNetCore;

namespace MetaDataAPI;

public static class DefaultServiceProvider
{
    private static readonly Lazy<IServiceProvider> LazyInstance = new(() =>
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        serviceCollection.AddFluentValidation([Assembly.GetExecutingAssembly()]);

        serviceCollection.AddSingleton<IRetryExecutor, RetryExecutor>();
        serviceCollection.AddSingleton<IHttpClientFactory, HttpClientFactory>();
        serviceCollection.AddSingleton<IWeb3Factory, Web3Factory>();
        serviceCollection.AddSingleton<IGraphQLClient, StrapiGraphQLClient>();
        serviceCollection.AddSingleton<IStrapiClient, StrapiClient>();
        serviceCollection.AddSingleton<IChainManager, StrapiChainManager>();
        serviceCollection.AddSingleton<IErc20Provider, Erc20Provider>();
        serviceCollection.AddSingleton<IErc20CacheService, Erc20CacheService>();
        serviceCollection.AddSingleton<ILockDealNFTService, LockDealNFTService>();

        return serviceCollection.BuildServiceProvider();
    });

    public static IServiceProvider Instance => LazyInstance.Value;
}