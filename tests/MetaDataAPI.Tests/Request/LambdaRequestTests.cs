using Xunit;
using System.Numerics;
using Amazon.Lambda.APIGatewayEvents;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Validation;

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
            "RawPath is required (expected format: '/{chainId}/{poolId}')."
        ];
        yield return
        [
            "/1/",
            "GET",
            0,
            0,
            false,
            "RawPath must be '/{chainId}/{poolId}'. The first path parameter is 'chainId', the second is 'poolId'. Received: '/1/'."
        ];
        yield return
        [
            "/invalid/2",
            "GET",
            0,
            0,
            false,
            "The first path parameter (chainId) must be a valid Int64. Received: 'invalid'."
        ];
        yield return
        [
            "/1/invalid",
            "GET",
            0,
            0,
            false,
            "The second path parameter (poolId) must be a valid Int64. Received: 'invalid'."
        ];
        yield return
        [
            "/1/2",
            "",
            0,
            0,
            false,
            "HTTP method is required."
        ];
        yield return
        [
            "/1/2",
            "POST",
            0,
            0,
            false,
            $"Allowed HTTP methods: ({string.Join(", ", LambdaRequestValidator.AllowedMethods)}). Received HTTP method: POST"
        ];
    }
}