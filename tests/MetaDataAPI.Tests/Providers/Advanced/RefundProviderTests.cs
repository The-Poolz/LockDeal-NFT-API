using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Advanced;
using MetaDataAPI.Storage;
using Nethereum.Hex.HexTypes;

namespace MetaDataAPI.Tests.Providers.Advanced;

public class RefundProviderTests : SetEnvironments
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var poolId = new BigInteger(0);
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody(HttpMock.DecimalsRequest)
            .RespondWith(HttpMock.DecimalsResponse);
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody($"{{\"jsonrpc\":\"2.0\",\"method\":\"eth_call\",\"params\":[{{\"to\":\"0x57e0433551460e85dfc5a5ddaff4db199d0f960a\",\"data\":\"{MethodSignatures.GetData + new HexBigInteger(poolId).HexValue[2..].PadLeft(64, '0')}\"}},\"latest\"],\"id\":0}}")
            .RespondWith(HttpMock.TimedResponse);

        var provider = new RefundProvider(poolId);

        provider.ParametersCount.Should().Be(2);
        provider.GetAttributes(new BigInteger(1), new BigInteger(100))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("RateToWei", new BigInteger(100), DisplayType.Number),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount", 0.000000000000000898m, DisplayType.Number, 0.000000000000000950m),
                new("StartTime", new BigInteger(1690286212), DisplayType.Date),
                new("FinishTime", new BigInteger(1690385286), DisplayType.Date),
            });
    }
}