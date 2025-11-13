using Polly;
using System.Reflection;
using Amazon.Lambda.Core;
using Flurl.Http.Configuration;
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
using IHttpClientFactory = Flurl.Http.Configuration.IHttpClientFactory;
using Polly.Retry;
using Polly.Timeout;

namespace MetaDataAPI;

public static class DefaultServiceProvider
{
    private static readonly Lazy<IServiceProvider> LazyInstance = new(() =>
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        serviceCollection.AddFluentValidation([Assembly.GetExecutingAssembly()]);

        serviceCollection.AddTransient<FailureOnlyLoggingHandler>();

        serviceCollection
            .AddHttpClient(HttpClientFactory.GenericClientName, client =>
            {
                client.Timeout = Timeout.InfiniteTimeSpan;
            })
            .AddHttpMessageHandler<FailureOnlyLoggingHandler>()
            .AddResilienceHandler("genericClient", pipeline =>
            {
                pipeline.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    Delay = TimeSpan.FromMilliseconds(200),
                    ShouldHandle = args =>
                    {
                        var exception = args.Outcome.Exception;

                        return new ValueTask<bool>(
                            exception is HttpRequestException
                                or TaskCanceledException
                                or TimeoutRejectedException
                        );
                    },
                    OnRetry = args =>
                    {
                        if (args.Outcome.Exception is { } ex)
                        {
                            var message =
                                $"[Retry] Attempt={args.AttemptNumber + 1}, Delay={args.RetryDelay}, Exception={ex.GetType().Name}: {ex.Message}";
                            LambdaLogger.Log(message);
                        }

                        return default;
                    }
                });

                pipeline.AddTimeout(TimeSpan.FromSeconds(3));
            });

        serviceCollection.AddSingleton<HttpClientFactory>();
        serviceCollection.AddSingleton<Services.Http.IHttpClientFactory>(
            provider => provider.GetRequiredService<HttpClientFactory>()
        );
        serviceCollection.AddSingleton<IRetryExecutor, RetryExecutor>();
        serviceCollection.AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();
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