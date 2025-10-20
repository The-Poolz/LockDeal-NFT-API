using Xunit;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Models.Errors;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests
{
    public class FunctionHandler
    {
        private readonly LambdaFunction _function = new();
        private readonly ILambdaContext _context = new TestLambdaContext();

        [Fact]
        internal void ShouldReturnOptionsResponse_WhenMethodIsOptions()
        {
            var request = new LambdaRequest(
                requestContext: new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
                {
                    Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                    {
                        Method = "OPTIONS"
                    }
                },
                rawPath: "/1/2/"
            );

            var response = _function.FunctionHandler(request, _context);

            response.Should().BeOfType<OptionsResponse>();
            response.StatusCode.Should().Be(204);
            response.Body.Should().BeEmpty();
        }

        [Fact]
        internal void ShouldIncludeCorsHeaders_InOptionsResponse()
        {
            var request = new LambdaRequest(
                requestContext: new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
                {
                    Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                    {
                        Method = "OPTIONS"
                    }
                },
                rawPath: "/1/2/"
            );

            var response = _function.FunctionHandler(request, _context);

            response.Headers.Should().ContainKey("Access-Control-Allow-Origin")
                .WhoseValue.Should().Be("*");
            response.Headers.Should().ContainKey("Access-Control-Allow-Methods")
                .WhoseValue.Should().Be("GET, OPTIONS");
            response.Headers.Should().ContainKey("Access-Control-Allow-Headers")
                .WhoseValue.Should().Be("Content-Type");
        }

        [Fact]
        internal void ShouldIncludeCorsHeaders_InValidationErrorResponse()
        {
            var request = new LambdaRequest(
                requestContext: new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
                {
                    Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                    {
                        Method = "GET"
                    }
                },
                rawPath: "/invalid/path/"
            );

            var response = _function.FunctionHandler(request, _context);

            response.Should().BeOfType<ValidationErrorResponse>();
            response.Headers.Should().ContainKey("Access-Control-Allow-Origin")
                .WhoseValue.Should().Be("*");
            response.Headers.Should().ContainKey("Access-Control-Allow-Methods")
                .WhoseValue.Should().Be("GET, OPTIONS");
            response.Headers.Should().ContainKey("Access-Control-Allow-Headers")
                .WhoseValue.Should().Be("Content-Type");
        }
    }
}
