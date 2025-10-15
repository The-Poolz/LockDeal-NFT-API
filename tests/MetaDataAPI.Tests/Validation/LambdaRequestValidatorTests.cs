using Xunit;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Validation;
using FluentValidation.TestHelper;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Tests.Validation;

public class LambdaRequestValidatorTests
{
    public class Validate
    {
        private readonly LambdaRequest _request = new(new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext(), string.Empty);
        private readonly LambdaRequestValidator _validator = new();

        [Fact]
        internal void ShouldHaveError_WhenQueryPathIsNull()
        {
            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.RawPath)
                .WithErrorMessage("RawPath is required (expected format: '/{chainId}/{poolId}').");
        }

        [Fact]
        public void ShouldHaveError_WhenChainIdIsMissing()
        {
            _request.RawPath = "/";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.RawPath)
                .WithErrorMessage("RawPath must be '/{chainId}/{poolId}'. The first path parameter is 'chainId', the second is 'poolId'. Received: '/'.");
        }

        [Fact]
        internal void ShouldHaveError_WhenChainIdIsInvalid()
        {
            _request.RawPath = "/invalid/1/";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.RawPath)
                .WithErrorMessage("The first path parameter (chainId) must be a valid Int64. Received: 'invalid'.");
        }

        [Fact]
        internal void ShouldHaveError_WhenPoolIdIsInvalid()
        {
            _request.RawPath = "/1/invalid/";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.RawPath)
                .WithErrorMessage("The second path parameter (poolId) must be a valid Int64. Received: 'invalid'.");
        }

        [Fact]
        internal void ShouldHaveError_WhenHttpMethodIsEmpty()
        {
            var request = new LambdaRequest(
                requestContext: new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
                {
                    Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription()
                },
                rawPath: "/1/2/"
            );

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.HttpMethod)
                .WithErrorMessage("HTTP method is required.");
        }

        [Fact]
        internal void ShouldHaveError_WhenHttpMethodIsInvalid()
        {
            var request = new LambdaRequest(
                requestContext: new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
                {
                    Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                    {
                        Method = "POST"
                    }
                },
                rawPath: "/1/2/"
            );

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.HttpMethod)
                .WithErrorMessage($"Allowed HTTP methods: ({string.Join(", ", LambdaRequestValidator.AllowedMethods)}). Received HTTP method: POST");
        }

        [Fact]
        internal void ShouldNotHaveAnyErrors_WhenRequestIsValid()
        {
            var request = new LambdaRequest(
                requestContext: new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
                {
                    Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                    {
                        Method = "GET"
                    }
                }, 
                rawPath: "/1/2/"
            );

            var result = _validator.TestValidate(request);

            result.IsValid.Should().BeTrue();
        }
    }
}