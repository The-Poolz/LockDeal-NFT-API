using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;

namespace MetaDataAPI.Tests.Providers.Simple;

public class TimedProviderTests
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var values = new BigInteger[] { 42, 1692106619, 1692106620, 100 };
        var provider = new TimedProvider(18, values);
        var result = provider.Attributes;

        provider.ParametersCount.Should().Be(4);
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", 0.000000000000000042m, DisplayType.Number, 0.0000000000000001m),
            new("StartTime", values[1], DisplayType.Date),
            new("FinishTime", values[2], DisplayType.Date)
        });
    }

    [Fact]
    public void GetDescription_ShouldExpectedDescription()
    {
        const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";
        var values = new BigInteger[] { 1000000000000000000, 1692106619, 1692106620, 2000000000000000000 };
        var provider = new TimedProvider(18, values);

        var result = provider.GetDescription(token);

        result.Should()
            .BeEquivalentTo($"This NFT governs a time-locked pool containing {1} units of the asset {token}. Withdrawals are permitted in a linear fashion beginning at {values[1]}, culminating in full access at {values[2]}.");
    }
}