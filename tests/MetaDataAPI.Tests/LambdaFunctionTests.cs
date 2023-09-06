using Xunit;
using System.Net;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Storage;
using Nethereum.Hex.HexTypes;
using MetaDataAPI.Tests.Helpers;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Tests;

public class LambdaFunctionTests : SetEnvironments
{
    [Fact]
    public void FunctionHandler_ShouldReturnCorrectResponse()
    {
        var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody(HttpMock.DecimalsRequest)
            .RespondWith(HttpMock.DecimalsResponse);
        var data = MethodSignatures.GetData + new HexBigInteger(0).HexValue[2..].PadLeft(64, '0');
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody($"{{\"jsonrpc\":\"2.0\",\"method\":\"eth_call\",\"params\":[{{\"to\":\"0x57e0433551460e85dfc5a5ddaff4db199d0f960a\",\"data\":\"{data}\"}},\"latest\"],\"id\":0}}")
            .RespondWith(HttpMock.DealResponse);

        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "0" } }
        };

        var response = LambdaFunction.FunctionHandler(request);

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
    }
}