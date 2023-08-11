using Xunit;
using System.Net;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Tests;

public class FunctionTests : SetEnvironments
{
    [Fact]
    public void FunctionHandler_ShouldReturnCorrectResponse()
    {
        const string rpcUrl = "https://endpoints.omniatech.io/v1/bsc/testnet/public";
        Environment.SetEnvironmentVariable("RPC_URL", rpcUrl);
        const string metadata = "0x0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338000000000000000000000000000000000000000000000000000000000000000000000000000000000000000057e0433551460e85dfc5a5ddaff4db199d0f960a00000000000000000000000066134461c865f824d294d8ca0d9080cc1acd05f600000000000000000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000";
        var rpcResponse = new JObject
        {
            { "result", metadata }
        };
        var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(rpcUrl)
            .RespondWithJson(rpcResponse);

        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "0" } }
        };

        var response = new Function().FunctionHandler(request);

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
        response.Body.Should().BeEquivalentTo(JObject.FromObject(
            new Erc721Metadata(new BasePoolInfo(
                provider: new Provider("0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338"),
                poolId: new BigInteger(0),
                owner: "0x57e0433551460e85dfc5a5ddaff4db199d0f960a",
                token: "0x66134461c865f824d294d8ca0d9080cc1acd05f6",
                new BigInteger[] { new(0) }
            ))
        ).ToString());
    }
}