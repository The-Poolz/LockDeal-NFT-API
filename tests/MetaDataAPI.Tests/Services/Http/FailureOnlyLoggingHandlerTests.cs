using Xunit;
using System.Net;
using FluentAssertions;
using MetaDataAPI.Services.Http;

namespace MetaDataAPI.Tests.Services.Http;

public class FailureOnlyLoggingHandlerTests
{
    [Fact]
    internal async Task ShouldReturnResponseWhenRequestIsSuccessful()
    {
        using var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        using var invoker = new HttpMessageInvoker(new FailureOnlyLoggingHandler(new StubHttpMessageHandler(expectedResponse)));
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test");

        var response = await invoker.SendAsync(request, CancellationToken.None);

        response.Should().BeSameAs(expectedResponse);
    }

    [Fact]
    internal async Task ShouldWrapHttpRequestExceptionWithMethodAndUrl()
    {
        using var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        using var invoker = new HttpMessageInvoker(new FailureOnlyLoggingHandler(new StubHttpMessageHandler(responseMessage)));
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/resource");

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => invoker.SendAsync(request, CancellationToken.None));

        exception.Message.Should().Contain("HTTP request failed");
        exception.Message.Should().Contain("METHOD: POST");
        exception.Message.Should().Contain("URL: https://example.test/resource");
        exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        exception.InnerException.Should().BeOfType<HttpRequestException>();
    }

    private sealed class StubHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}