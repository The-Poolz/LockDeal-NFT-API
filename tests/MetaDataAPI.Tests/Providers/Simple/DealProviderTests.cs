using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;

namespace MetaDataAPI.Tests.Providers.Simple;

public class DealProviderTests
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var provider = new DealProvider();
        var values = new BigInteger[] { 42 };
        var result = provider.GetAttributes(values).ToArray();

        provider.ParametersCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", 0.000000000000000042m, "number")
        });
    }
}