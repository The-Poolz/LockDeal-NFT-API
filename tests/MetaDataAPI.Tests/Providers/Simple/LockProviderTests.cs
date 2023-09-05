using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;

namespace MetaDataAPI.Tests.Providers.Simple;

public class LockProviderTests
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var values = new BigInteger[] { 42, 1692106619 };
        var provider = new LockProvider(18, values);
        var result = provider.Attributes;

        provider.ParametersCount.Should().Be(2);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", 0.000000000000000042m, DisplayType.Number),
            new("StartTime", values[1], DisplayType.Date)
        });
    }

    [Fact]
    public void GetDescription_ShouldExpectedDescription()
    {
        const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";
        var values = new BigInteger[] { 1000000000000000000, 1692106619 };
        var provider = new LockProvider(18, values);

        var result = provider.GetDescription(token);

        result.Should()
            .BeEquivalentTo($"This NFT securely locks {1} units of the asset {token}. Access to these assets will commence on the designated start time of {values[1]}.");
    }
}