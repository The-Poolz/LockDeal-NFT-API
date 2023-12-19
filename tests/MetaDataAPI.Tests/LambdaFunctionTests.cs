using Xunit;
using System.Net;
using FluentAssertions;
using MetaDataAPI.Utils;
using MetaDataAPI.Storage;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json.Linq;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    private const int start = 0;
    private const int end = 13;

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
        var lambda = SetupLambdaFunction();
        var request = CreateRequest(id);

        var response = lambda.FunctionHandler(request);

        AssertResponse(id, response);
    }

    public static IEnumerable<object[]> TestCases()
    {
        for (var i = start; i <= end; i++)
        {
            yield return new object[] { i };
        }
    }

    private static LambdaFunction SetupLambdaFunction()
    {
        var mockRpcCaller = new MockRpcCaller();
        var factory = new ProviderFactory(mockRpcCaller);
        var dynamoDb = new DynamoDb(MockAmazonDynamoDB.MockClient());
        return new LambdaFunction(factory, dynamoDb);
    }

    private static APIGatewayProxyRequest CreateRequest(int id)
    {
        return new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", id.ToString() } }
        };
    }

    private static void AssertResponse(int id, APIGatewayProxyResponse response)
    {
        response.Body.Should().Contain($"Lock Deal NFT Pool: {id}");
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
        response.Body.Should().Contain(StaticResults.ExpectedDescription[id]);

        if (StaticResults.ExpectedAttributes.ContainsKey(id))
        {
            var responseBody = JObject.Parse(response.Body);
            var expectedAttributes = JArray.FromObject(StaticResults.ExpectedAttributes[id]);
            var actualAttributes = responseBody["attributes"] as JArray;
            actualAttributes.Should().BeEquivalentTo(expectedAttributes, 
                $"\nExpected attributes to be:\n{expectedAttributes}, \nBut we received:\n{actualAttributes}\n");
        }
    }

    [Theory]
    [MemberData(nameof(ErrorCases))]
    public void FunctionHandler_ShouldThrowInvalidOperationException(
        string expectedError,
        Dictionary<string, string> queryStringParameters,
        bool useErrorMock)
    {
        var mockRpcCaller = new MockRpcCaller(useErrorMock);
        var factory = new ProviderFactory(mockRpcCaller);
        var dynamoDb = new DynamoDb(MockAmazonDynamoDB.MockClient());
        var lambda = new LambdaFunction(factory, dynamoDb);

        var request = new APIGatewayProxyRequest { QueryStringParameters = queryStringParameters };

        var result = lambda.FunctionHandler(request);

        result.Body.Should().Be(expectedError);
    }

    public static IEnumerable<object[]> ErrorCases()
    {
        // No id parameter
        yield return new object[]
        {
            ErrorMessages.MissingIdMessage,
            new Dictionary<string, string>(),
            false
        };

        // Invalid id parameter
        yield return new object[]
        {
            ErrorMessages.InvalidIdMessage,
            new Dictionary<string, string> { { "id", "invalid" } },
            false
        };

        // Id not the same in response
        yield return new object[]
        {
            ErrorMessages.InvalidResponseMessage,
            new Dictionary<string, string> { { "id", "123" } },
            false
        };

        // Pool id not in range
        yield return new object[]
        {
            ErrorMessages.PoolIdNotInRangeMessage,
            new Dictionary<string, string> { { "id", "9999999" } },
            false
        };

        // Failed to create provider
        yield return new object[]
        {
            ErrorMessages.FailedToCreateProviderMessage,
            new Dictionary<string, string> { { "id", "1" } },
            true
        };
    }
}