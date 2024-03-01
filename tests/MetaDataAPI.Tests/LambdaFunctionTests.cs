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
    public LambdaFunctionTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
        Environment.SetEnvironmentVariable("NAME_OF_STAGE", "test");
    }

    [Fact] 
    public void Ctor_WithoutParameters()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-west-2");
        var lambda = new LambdaFunction();
        lambda.Should().NotBeNull();
    }

    [Fact]
    public void FunctionHandler_ShouldReturnCorrectResponse()
    {
        var id = 0;
        var lambda = SetupLambdaFunction();
        var request = CreateRequest(id);

        var response = lambda.FunctionHandler(request);

        response.Body.Should().Contain($"Lock Deal NFT Pool: {id}");
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
        response.Body.Should().Contain("This NFT has been fully withdrawn");
    }

    private static LambdaFunction SetupLambdaFunction()
    {
        //var mockRpcCaller = new MockRpcCaller();
        var factory = new ProviderFactory();//need to mock this
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
}