using Xunit;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Validation;
using FluentValidation.TestHelper;

namespace MetaDataAPI.Tests.Validation;

public class LambdaRequestValidatorTests
{
    public class Validate
    {
        private readonly LambdaRequest _request = new(string.Empty, string.Empty);
        private readonly LambdaRequestValidator _validator = new();

        [Fact]
        internal void ShouldHaveError_WhenQueryPathIsNull()
        {
            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.Path)
                .WithErrorMessage(ValidatorErrorsMessages.PathRequired());
        }

        [Fact]
        public void ShouldHaveError_WhenChainIdIsMissing()
        {
            _request.Path = "/metadata/";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.Path)
                .WithErrorMessage(ValidatorErrorsMessages.PathWrongFormat(_request.Path));
        }

        [Fact]
        public void ShouldHaveError_WhenPathIsNotAllowed()
        {
            _request.Path = "/wrong/1/2";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.Path)
                .WithErrorMessage(ValidatorErrorsMessages.PathNotAllowed(_request.Path));
        }

        [Fact]
        internal void ShouldHaveError_WhenChainIdIsInvalid()
        {
            _request.Path = "/metadata/invalid/1/";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.Path)
                .WithErrorMessage(ValidatorErrorsMessages.ChainIdInvalid(_request.Path));
        }

        [Fact]
        internal void ShouldHaveError_WhenPoolIdIsInvalid()
        {
            _request.Path = "/metadata/1/invalid/";

            var result = _validator.TestValidate(_request);

            result.ShouldHaveValidationErrorFor(r => r.Path)
                .WithErrorMessage(ValidatorErrorsMessages.PoolIdInvalid(_request.Path));
        }

        [Fact]
        internal void ShouldHaveError_WhenHttpMethodIsEmpty()
        {
            var request = new LambdaRequest(
                httpMethod: string.Empty,
                path: "/metadata/1/2/"
            );

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.HttpMethod)
                .WithErrorMessage(ValidatorErrorsMessages.HttpMethodRequired());
        }

        [Fact]
        internal void ShouldHaveError_WhenHttpMethodIsInvalid()
        {
            var request = new LambdaRequest(
                httpMethod: "POST",
                path: "/metadata/1/2/"
            );

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.HttpMethod)
                .WithErrorMessage(ValidatorErrorsMessages.HttpMethodNotAllowed("POST", LambdaRequest.AllowedMethods));
        }

        [Fact]
        internal void ShouldNotHaveAnyErrors_WhenRequestIsForFavicon()
        {
            var request = new LambdaRequest(
                httpMethod: "GET",
                path: "/favicon.ico"
            );

            var result = _validator.TestValidate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        internal void ShouldNotHaveAnyErrors_WhenRequestIsValid()
        {
            var request = new LambdaRequest(
                httpMethod: "GET",
                path: "/metadata/1/2/"
            );

            var result = _validator.TestValidate(request);

            result.IsValid.Should().BeTrue();
        }
    }
}