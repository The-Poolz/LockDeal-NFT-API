using Moq;
using Xunit;
using ImageAPI.Utils;
using FluentAssertions;
using Amazon.Lambda.APIGatewayEvents;
using SixLabors.ImageSharp;

namespace ImageAPI.Test;

public class FunctionHandlerTests
{
    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_WrongInput()
    {
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "id", "not number" }
            }
        };

        var result = new LambdaFunction().Run(request);

        result.Should().BeEquivalentTo(ResponseBuilder.WrongInput());
    }

    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_ImageResponse()
    {
        var expected = ResponseBuilder.ImageResponse("base64ImageHere");
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "id", "1" }
            }
        };

        var result = new LambdaFunction().Run(request);

        result.Body.Should().NotBe(string.Empty);
        result.Headers.Should().BeEquivalentTo(expected.Headers);
        result.IsBase64Encoded.Should().Be(expected.IsBase64Encoded);
        result.StatusCode.Should().Be(expected.StatusCode);
    }

    [Fact]
    internal void FunctionHandler_ShouldReturnResponse_GeneralError()
    {
        var imageProcessor = new Mock<ImageProcessor>();
        imageProcessor
            .Setup(x => x.CreateTextOptions(It.IsAny<PointF>()))
            .Throws<InvalidOperationException>();
        var dynamoDb = new Mock<DynamoDb>();

        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>
            {
                { "id", "1" }
            }
        };

        var result = new LambdaFunction(imageProcessor.Object, dynamoDb.Object).Run(request);

        result.Should().BeEquivalentTo(ResponseBuilder.GeneralError());
    }
}