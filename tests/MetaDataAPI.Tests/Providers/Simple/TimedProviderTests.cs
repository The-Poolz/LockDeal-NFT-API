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
        var provider = new TimedProvider(18);
        var values = new BigInteger[] { 42, 1692106619, 1692106620, 100 };
        var result = provider.GetAttributes(values).ToArray();

        provider.ParametersCount.Should().Be(4);
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", 0.000000000000000042m, DisplayType.Number, 0.0000000000000001m),
            new("StartTime", values[1], DisplayType.Date),
            new("FinishTime", values[2], DisplayType.Date)
        });
    }
}