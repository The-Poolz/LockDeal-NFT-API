using Moq;
using Xunit;
using FluentAssertions;
using System.Net.Http.Headers;
using MetaDataAPI.Services.Http;
using MetaDataAPI.Services.Strapi;

namespace MetaDataAPI.Tests.Services.Strapi;

public class StrapiGraphQLClientTests
{
    [Fact]
    internal void ShouldCreateHttpClientWithNoCacheHeaders()
    {
        const string endpoint = "https://strapi.test/graphql";
        Environment.SetEnvironmentVariable(nameof(Env.GRAPHQL_STRAPI_URL), endpoint);

        string? requestedUrl = null;
        Action<HttpRequestHeaders>? configureHeaders = null;

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(factory => factory.Create(It.IsAny<string>(), It.IsAny<Action<HttpRequestHeaders>>()))
            .Callback<string, Action<HttpRequestHeaders>>((url, configure) =>
            {
                requestedUrl = url;
                configureHeaders = configure;
            })
            .Returns(new HttpClient(new HttpClientHandler()));

        using var client = new StrapiGraphQLClient(httpClientFactory.Object);

        requestedUrl.Should().Be(endpoint);
        configureHeaders.Should().NotBeNull();

        var headers = new HttpRequestMessage().Headers;
        configureHeaders!(headers);

        headers.CacheControl.Should().NotBeNull();
        headers.CacheControl!.NoCache.Should().BeTrue();
        headers.CacheControl.NoStore.Should().BeTrue();
        headers.CacheControl.MustRevalidate.Should().BeTrue();
    }
}