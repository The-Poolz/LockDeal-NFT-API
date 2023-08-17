using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers.Advanced;

public class CollateralProviderTests : SetEnvironments
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.DealResponse);

        var provider = new CollateralProvider(new BigInteger(0));

        provider.ParametersCount.Should().Be(2);
        provider.GetAttributes(new BigInteger(100), new BigInteger(1690286212))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(100), DisplayType.Number),
                new("FinishTime", new BigInteger(1690286212), DisplayType.Date),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount_1", new BigInteger(0), DisplayType.Number),
                new("LeftAmount_2", new BigInteger(0), DisplayType.Number),
                new("LeftAmount_3", new BigInteger(0), DisplayType.Number),
            });
    }
}