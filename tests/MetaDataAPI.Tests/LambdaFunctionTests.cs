using Xunit;
using System.Net;
using FluentAssertions;
using MetaDataAPI.Utils;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    private const int start = 0;
    private const int end = 20;

    public LambdaFunctionTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
    }

    [Fact] 
    public void Ctor_WithoutParameters()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
        var lambda = new LambdaFunction();
        lambda.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FunctionHandler_ShouldReturnCorrectResponse(int id)
    {
        var mockRpcCaller = new MockRpcCaller();
        var factory = new ProviderFactory(mockRpcCaller);
        var dynamoDb = new DynamoDb(MockAmazonDynamoDB.MockClient());
        var lambda = new LambdaFunction(factory, dynamoDb);
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
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string>()
        };

        var result = new LambdaFunction().FunctionHandler(request);

        result.Body.Should().Be(ErrorMessages.missingIdMessage);
    }

    [Fact]
    public void FunctionHandler_ShouldThrowInvalidOperationExceptionWhenIdIsInvalid()
    {
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "invalid" } }
        };

        var result = new LambdaFunction().FunctionHandler(request);

        result.Body.Should().Be(ErrorMessages.invalidIdMessage);
    }

    [Fact]
    public void FunctionHandler_ShouldThrowInvalidOperationExceptionWhenIdNotTheSameInResponse()
    {
        var mockRpcCaller = new MockRpcCaller();
        var factory = new ProviderFactory(mockRpcCaller);
        var function = new LambdaFunction(factory, new DynamoDb(MockAmazonDynamoDB.MockClient()));
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "123" } }
        };

        var result = function.FunctionHandler(request);

        result.Body.Should().Be(ErrorMessages.invalidResponseMessage);
    }

    [Fact]
    public void FunctionHandler_ShouldThrowInvalidOperationExceptionWhenPoolIdNotInRange()
    {
        var mockRpcCaller = new MockRpcCaller();
        var factory = new ProviderFactory(mockRpcCaller);
        var function = new LambdaFunction(factory, new DynamoDb(MockAmazonDynamoDB.MockClient()));
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "9999999" } }
        };

        var result = function.FunctionHandler(request);

        result.Body.Should().Be(ErrorMessages.poolIdNotInRangeMessage);
    }

    [Fact]
    public void FunctionHandler_ShouldThrowInvalidOperationExceptionWhenFailedToCreateProvider()
    {
        var mockRpcCaller = new MockRpcError();
        var factory = new ProviderFactory(mockRpcCaller);
        var function = new LambdaFunction(factory, new DynamoDb(MockAmazonDynamoDB.MockClient()));
        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "1" } }
        };

        var result = function.FunctionHandler(request);

        result.Body.Should().Be(ErrorMessages.failedToCreateProviderMessage);
    }
}