using Xunit;
using FluentAssertions;
using System.Reflection;
using MetaDataAPI.Services.Http;

namespace MetaDataAPI.Tests.Services.Http;

public class HttpClientFactoryTests
{
    [Fact]
    internal void ShouldCreateHttpClientWithBaseAddressAndFailureLoggingHandler()
    {
        const string url = "https://example.test/";
        var factory = new HttpClientFactory();

        using var client = factory.Create(url);

        client.BaseAddress.Should().Be(new Uri(url));

        var handlerField = typeof(HttpMessageInvoker).GetField("_handler", BindingFlags.Instance | BindingFlags.NonPublic);
        handlerField.Should().NotBeNull();

        var handler = handlerField!.GetValue(client);
        handler.Should().BeOfType<FailureOnlyLoggingHandler>();

        var innerHandlerField = typeof(DelegatingHandler).GetField("_innerHandler", BindingFlags.Instance | BindingFlags.NonPublic);
        innerHandlerField.Should().NotBeNull();

        var innerHandler = innerHandlerField!.GetValue(handler);
        innerHandler.Should().BeOfType<HttpClientHandler>();
    }

    [Fact]
    internal void ShouldApplyHeaderConfigurationWhenProvided()
    {
        var factory = new HttpClientFactory();
        const string url = "https://example.test/";
        const string headerName = "X-Test";
        const string headerValue = "expected-value";

        using var client = factory.Create(url, headers => headers.Add(headerName, headerValue));

        client.BaseAddress.Should().Be(new Uri(url));
        client.DefaultRequestHeaders.TryGetValues(headerName, out var values).Should().BeTrue();
        values.Should().ContainSingle().Which.Should().Be(headerValue);
    }
}