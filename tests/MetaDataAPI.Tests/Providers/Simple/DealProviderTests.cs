using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;

namespace MetaDataAPI.Tests.Providers.Simple;

public class DealProviderTests
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var values = new BigInteger[] { 42 };
        var provider = new DealProvider(18, values);
        var result = provider.Attributes;

        provider.ParametersCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", 0.000000000000000042m, DisplayType.Number)
        });
    }

    [Fact]
    public void GetDescription_ShouldExpectedDescription()
    {
        const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";
        var values = new BigInteger[] { 0 };
        var provider = new DealProvider(18, values);

        var result = provider.GetDescription(token);

        result.Should()
            .BeEquivalentTo($"This NFT represents immediate access to {values[0]} units of the specified asset {token}.");
    }
}