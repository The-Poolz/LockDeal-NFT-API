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
        var values = new BigInteger[] { new(100), new(1690286212) };

        var provider = new CollateralProvider(new BigInteger(0), 18, values);

        provider.ParametersCount.Should().Be(2);
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", 0.0000000000000001m, DisplayType.Number),
                new("FinishTime", new BigInteger(1690286212), DisplayType.Date),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount_1", 0m, DisplayType.Number),
                new("LeftAmount_2", 0m, DisplayType.Number),
                new("LeftAmount_3", 0m, DisplayType.Number),
            });
    }

    [Fact]
    public void GetDescription_ShouldExpectedDescription()
    {
        const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";
        var values = new BigInteger[] { 1000, 1690385286 };
        var httpTest = new HttpTest();
        httpTest.SetupRpcCall(13, HttpMock.DealResponse);
        httpTest.SetupRpcCall(14, HttpMock.DealResponse);
        httpTest.SetupRpcCall(15, HttpMock.DealResponse);
        var provider = new CollateralProvider(12, 18, values);

        var result = provider.GetDescription(token);

        result.Should()
            .BeEquivalentTo($"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens. " +
                            $"It holds {0} for the main coin collector, {0} for the token collector," +
                            $" and {0} for the main coin holder, valid until {1690385286}.");
    }
}