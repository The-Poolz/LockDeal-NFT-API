using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Request;

namespace MetaDataAPI.Tests.Request;

public class LambdaRequestTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    internal void ShouldSetPropertiesAndReturnValidationResult(
        Dictionary<string, string> queryStringParameters,
        BigInteger expectedChainId,
        BigInteger expectedPoolId,
        bool isValid,
        string errorMessage
    )
    {
        var request = new LambdaRequest
        {
            QueryStringParameters = queryStringParameters
        };

        var validationResult = request.ValidationResult;

        request.ChainId.Should().Be(expectedChainId);
        request.PoolId.Should().Be(expectedPoolId);
        if (isValid)
        {
            validationResult.Should().BeNull();
        }
        else
        {
            validationResult.Should().NotBeNull();
            validationResult!.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(errorMessage);
        }
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return new object[]
        {
            new Dictionary<string, string> { { "chainId", "1" }, { "poolId", "2" } },
            new BigInteger(1),
            new BigInteger(2),
            true,
            string.Empty
        };
        yield return new object[]
        {
            new Dictionary<string, string> { { "chainId", "invalid" }, { "poolId", "2" } },
            new BigInteger(-1),
            new BigInteger(-1),
            false,
            "Query string parameter 'chainId' must be a valid BigInteger."
        };
        yield return new object[]
        {
            new Dictionary<string, string> { { "chainId", "1" }, { "poolId", "invalid" } },
            new BigInteger(-1),
            new BigInteger(-1),
            false,
            "Query string parameter 'poolId' must be a valid BigInteger."
        };
        yield return new object[]
        {
            new Dictionary<string, string> { { "chainId", "1" } },
            new BigInteger(-1),
            new BigInteger(-1),
            false,
            "Query string parameter 'poolId' is required."
        };
        yield return new object[]
        {
            new Dictionary<string, string>(),
            new BigInteger(-1),
            new BigInteger(-1),
            false,
            "Query string parameter 'chainId' is required."
        };
        yield return new object[]
        {
            null!,
            new BigInteger(-1),
            new BigInteger(-1),
            false,
            "Query string parameters are required."
        };
    }
}