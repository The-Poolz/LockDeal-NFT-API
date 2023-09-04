using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers.Advanced;

public class RefundProviderTests : SetEnvironments
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var poolId = new BigInteger(0);
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.TimedResponse);
        var values = new BigInteger[] { new(1), new(100000000000000000) };

        var provider = new RefundProvider(poolId, 18, values);

        provider.ParametersCount.Should().Be(2);
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("Rate", 0.1m, DisplayType.Number),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount", 0.000000000000000898m, DisplayType.Number, 0.000000000000000950m),
                new("StartTime", new BigInteger(1690286212), DisplayType.Date),
                new("FinishTime", new BigInteger(1690385286), DisplayType.Date),
            });
    }
}