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
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.TimedResponse);

        var provider = new RefundProvider(new BigInteger(0));

        provider.ParametersCount.Should().Be(2);
        provider.GetAttributes(new BigInteger(1), new BigInteger(100))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("RateToWei", new BigInteger(100), DisplayType.Number),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount", new BigInteger(898), DisplayType.Number, new BigInteger(950)),
                new("StartTime", new BigInteger(1690286212), DisplayType.Date),
                new("FinishTime", new BigInteger(1690385286), DisplayType.Date),
            });
    }
}