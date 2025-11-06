using Moq;
using Xunit;
using MediatR;
using FluentAssertions;
using FluentValidation;
using MetaDataAPI.Models;
using MetaDataAPI.Routing;
using MetaDataAPI.Validation;
using MetaDataAPI.Routing.Requests;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Tests.Request;

public class RouteApplicationLoadBalancerRequestHandlerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly RouteApplicationLoadBalancerRequestHandler _handler;

    public RouteApplicationLoadBalancerRequestHandlerTests()
    {
        _handler = new RouteApplicationLoadBalancerRequestHandler(_mediator.Object);
    }

    [Fact]
    public async Task ShouldReturnOptionsResponse_WhenHttpMethodIsOptions()
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.OptionsMethod
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeOfType<OptionsResponse>();
        _mediator.Verify(x => x.Send(It.IsAny<GetMetadataRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldReturnFaviconResponse_WhenPathIsForFavicon()
    {
        FaviconLambdaResponse.BytesProvider = () => [0x00, 0x01, 0x02];

        try
        {
            var albRequest = new ApplicationLoadBalancerRequest
            {
                HttpMethod = LambdaRoutes.GetMethod,
                Path = LambdaRoutes.FaviconPath
            };
            var request = new RouteApplicationLoadBalancerRequest(albRequest);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeOfType<FaviconLambdaResponse>();
            _mediator.Verify(x => x.Send(It.IsAny<GetMetadataRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        finally
        {
            FaviconLambdaResponse.BytesProvider = FaviconLambdaResponse.BytesProvider;
        }
    }

    [Fact]
    public async Task ShouldSendGetMetadataRequest_WhenPathIsForMetadata()
    {
        var expectedResponse = new OptionsResponse();
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.GetMethod,
            Path = "/metadata/1/2"
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        _mediator
            .Setup(x => x.Send(It.Is<GetMetadataRequest>(r => r.Path == albRequest.Path), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeSameAs(expectedResponse);
    }

    [Fact]
    public async Task ShouldThrowValidationException_WhenPathIsNotAllowed()
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.GetMethod,
            Path = "/not-allowed"
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var action = async () => await _handler.Handle(request, CancellationToken.None);

        await action.Should()
            .ThrowAsync<ValidationException>()
            .WithMessage(ValidatorErrorsMessages.PathNotAllowed(albRequest.Path));
    }
}