using Xunit;
using System.Net;
using FluentAssertions;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;
using System.Collections.Generic;
using System;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    private const int start = 0;
    private const int end = 20;

    [Fact] 
    public void Ctor_WithoutParameters()
    {
        var lambda = new LambdaFunction();
        lambda.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FunctionHandler_ShouldReturnCorrectResponse(int id)
    {
        var mockRpcCaller = new MockRpcCaller();
        var factory = new ProviderFactory(mockRpcCaller);
        var lambda = new LambdaFunction(factory);
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", id.ToString() } }
        };

        var response = lambda.FunctionHandler(request);

        response.Body.Should().Contain($"Lock Deal NFT Pool: {id}");
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
    }

    public static IEnumerable<object[]> TestCases()
    {
        for (var i = start; i <= end; i++)
        {
            yield return new object[] { i };
        }
    }

    [Fact]
    public void FunctionHandler_ShouldThrowInvalidOperationExceptionWhenIdIsMissing()
    {
        var lambda = new LambdaFunction();
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>()
        };

        var exception = Assert.Throws<InvalidOperationException>(() => lambda.FunctionHandler(request));

        exception.Message.Should().Be("Invalid request. The 'id' parameter is missing.");
    }

    [Fact]
    public void FunctionHandler_ShouldThrowInvalidOperationExceptionWhenIdIsInvalid()
    {
        var lambda = new LambdaFunction();
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "invalid" } }
        };

        var exception = Assert.Throws<InvalidOperationException>(() => lambda.FunctionHandler(request));

        exception.Message.Should().Be("Invalid request. The 'id' parameter is not a valid BigInteger.");
    }
}