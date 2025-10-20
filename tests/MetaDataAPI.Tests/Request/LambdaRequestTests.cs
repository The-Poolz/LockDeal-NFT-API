using Xunit;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Validation;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Tests.Request;

public class LambdaRequestTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    internal void ShouldSetPropertiesAndReturnValidationResult(
        string rawPath,
        string httpMethod,
        long expectedChainId,
        long expectedPoolId,
        bool isValid,
        string errorMessage
    )
    {
        var request = new LambdaRequest(
            new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
            {
                Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                {
                    Method = httpMethod
                }
            },
            rawPath
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
            "/1/2",
            "GET",
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
            LambdaRequestValidatorErrors.RawPathRequired()
        ];
        yield return
        [
            "/1/",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.RawPathWrongFormat("/1/")
        ];
        yield return
        [
            "/invalid/2",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.ChainIdInvalid("/invalid/2")
        ];
        yield return
        [
            "/1/invalid",
            "GET",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.PoolIdInvalid("/1/invalid")
        ];
        yield return
        [
            "/1/2",
            "",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.HttpMethodRequired()
        ];
        yield return
        [
            "/1/2",
            "POST",
            0,
            0,
            false,
            LambdaRequestValidatorErrors.HttpMethodNotAllowed("POST", LambdaRequestValidator.AllowedMethods)
        ];
        yield return
        [
            "/1/2",
            "OPTIONS",
            1,
            2,
            true,
            string.Empty
        ];
    }
}