using Xunit;
using System.Net;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;
using System.Collections.Generic;
using System;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    [Fact] 
    public void FunctionHandler_DefultCtor()
    {
        var lambda = new LambdaFunction();
        lambda.Should().NotBeNull();
    }
    [Fact]
    public void TestBasePoolInfo_Ctor()
    {
        var info = new BasePoolInfo(StaticResults.MetaData[0][2..],new ProviderFactory(new MockRpcCaller()));
        info.Should().NotBeNull();
        info.Params.Should().NotBeNull();
        info.VaultId.Should().NotBeNull();
        info.Owner.Should().NotBeNull();
    }

    const int start = 0;
    const int end = 20;
    internal static MockRpcCaller caller = new ();
    [Theory]
    [MemberData(nameof(TestCases))]
    public void FunctionHandler_ShouldReturnCorrectResponsea(int id)
    {   
        var factory = new ProviderFactory(caller);
        var lambda = new LambdaFunction(factory);
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", id.ToString() } }
        };

        var response = lambda.FunctionHandler(request);

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
    }

    public static IEnumerable<object[]> TestCases()
    {
        for (int i = start; i <= end; i++)
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