using Xunit;
using System.Net;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Providers;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    const int start = 0;
    const int end = 12;
    internal static MockRpcCaller caller = MockRpcCaller.InstallFullTest();
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
}