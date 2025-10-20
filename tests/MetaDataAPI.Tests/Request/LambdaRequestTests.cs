using Xunit;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Validation;

namespace MetaDataAPI.Tests.Request;

public class LambdaRequestTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    internal void ShouldSetPropertiesAndReturnValidationResult(
        string path,
        string httpMethod,
        long expectedChainId,
        long expectedPoolId,
        bool isValid,
        string errorMessage
    )
    {
        var request = new LambdaRequest(
            httpMethod,
            path
        );

        var validationResult = request.ValidationResult;

        request.ChainId.Should().Be(expectedChainId);
        request.PoolId.Should().Be(expectedPoolId);
        if (isValid)
        {
            validationResult.IsValid.Should().BeTrue();
        }
        else
        {
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(errorMessage);
        }
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return
        [
            "/metadata/1/2",
            "GET",
            1,
            2,
            true,
            string.Empty
        ];
        yield return
        [
            "/metadata/1/2",
            "OPTIONS",
            1,
            2,
            true,
            string.Empty
        ];
        yield return
        [
            "",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.PathRequired()
        ];
        yield return
        [
            "/metadata/1/",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.PathWrongFormat("/metadata/1/")
        ];
        yield return
        [
            "/wrong/1/2",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.PathWrongFormat("/wrong/1/2")
        ];
        yield return
        [
            "/metadata/invalid/2",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.ChainIdInvalid("/metadata/invalid/2")
        ];
        yield return
        [
            "/metadata/1/invalid",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.PoolIdInvalid("/metadata/1/invalid")
        ];
        yield return
        [
            "/metadata/1/2",
            "",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.HttpMethodRequired()
        ];
        yield return
        [
            "/metadata/1/2",
            "POST",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.HttpMethodNotAllowed("POST", LambdaRequest.AllowedMethods)
        ];
    }
}