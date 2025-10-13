using Xunit;
using FluentAssertions;
using MetaDataAPI.Request;
using MetaDataAPI.Validation;
using FluentValidation.TestHelper;

namespace MetaDataAPI.Tests.Validation;

public class LambdaRequestValidatorTests
{
    public class Validate
    {
        private readonly LambdaRequest _request = new();
        private readonly LambdaRequestValidator _validator = new();

        [Fact]
        internal void ShouldHaveError_WhenQueryStringParametersIsNull()
        {
            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.QueryStringParameters)
                .WithErrorMessage("Query string parameters are required.");
        }

        [Fact]
        public void ShouldHaveError_WhenChainIdIsMissing()
        {
            _request.QueryStringParameters = new Dictionary<string, string>();

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.QueryStringParameters)
                .WithErrorMessage("Query string parameter 'chainId' is required.");
        }

        [Fact]
        internal void ShouldHaveError_WhenChainIdIsInvalid()
        {
            _request.QueryStringParameters = new Dictionary<string, string>
            {
                { "chainId", "invalid" }
            };

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.QueryStringParameters)
                .WithErrorMessage("Query string parameter 'chainId' must be a valid Int64.");
        }

        [Fact]
        internal void ShouldHaveError_WhenPoolIdIsMissing()
        {
            _request.QueryStringParameters = new Dictionary<string, string>
            {
                { "chainId", "1" }
            };

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.QueryStringParameters)
                .WithErrorMessage("Query string parameter 'poolId' is required.");
        }

        [Fact]
        internal void ShouldHaveError_WhenPoolIdIsInvalid()
        {
            _request.QueryStringParameters = new Dictionary<string, string>
            {
                { "chainId", "1" },
                { "poolId", "invalid" }
            };

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.QueryStringParameters)
                .WithErrorMessage("Query string parameter 'poolId' must be a valid Int64.");
        }

        [Fact]
        internal void ShouldNotHaveAnyErrors_WhenRequestIsValid()
        {
            _request.QueryStringParameters = new Dictionary<string, string>
            {
                { "chainId", "1" },
                { "poolId", "2" }
            };

            var result = _validator.TestValidate(_request);

            result.IsValid.Should().BeTrue();
        }
    }
}