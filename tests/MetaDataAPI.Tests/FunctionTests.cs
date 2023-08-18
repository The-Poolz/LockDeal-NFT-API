using Xunit;
using System.Net;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Tests;

public class FunctionTests : SetEnvironments
{
    [Fact]
    public void FunctionHandler_ShouldReturnCorrectResponse()
    {
        var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.DealResponse);

        var request = new APIGatewayProxyRequest
        {
            QueryStringParameters = new Dictionary<string, string> { { "id", "0" } }
        };

        var response = new Function().FunctionHandler(request);

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Headers.Should().Contain(new KeyValuePair<string, string>("Content-Type", "application/json"));
        response.Body.Should().BeEquivalentTo(JObject.FromObject(
            new Erc721Metadata(new BasePoolInfo(
                provider: new DealProvider(18),
                poolId: new BigInteger(0),
                owner: "0x57e0433551460e85dfc5a5ddaff4db199d0f960a",
                token: "0x66134461c865f824d294d8ca0d9080cc1acd05f6",
                new BigInteger[] { new(0) }
            ))
        ).ToString());
    }
}