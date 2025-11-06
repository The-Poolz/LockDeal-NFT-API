using Xunit;
using FluentAssertions;
using MetaDataAPI.Validation;
using FluentValidation.TestHelper;
using MetaDataAPI.Routing.Requests;

namespace MetaDataAPI.Tests.Validation;

public class GetMetadataRequestValidatorTests
{
    private readonly GetMetadataRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("/metadata/1")]
    [InlineData("/metadata/1/2/3")]
    public void ShouldHaveError_WhenPathHasUnexpectedNumberOfSegments(string path)
    {
        var request = new GetMetadataRequest(path);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Path)
            .WithErrorMessage(ValidatorErrorsMessages.PathWrongFormat(path));
    }

    [Fact]
    public void ShouldHaveError_WhenChainIdIsInvalid()
    {
        const string path = "/metadata/invalid/2";
        var request = new GetMetadataRequest(path);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Path)
            .WithErrorMessage(ValidatorErrorsMessages.ChainIdInvalid(path));
    }

    [Fact]
    public void ShouldHaveError_WhenPoolIdIsInvalid()
    {
        const string path = "/metadata/1/invalid";
        var request = new GetMetadataRequest(path);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Path)
            .WithErrorMessage(ValidatorErrorsMessages.PoolIdInvalid(path));
    }

    [Fact]
    public void ShouldValidateSuccessfully_WhenPathIsValid()
    {
        const string path = "/metadata/1/2";
        var request = new GetMetadataRequest(path);

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }
}