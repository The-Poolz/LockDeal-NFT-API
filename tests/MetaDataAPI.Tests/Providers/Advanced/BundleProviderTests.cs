using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers.Advanced;

public class BundleProviderTests : SetEnvironments
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.TimedResponse);

        var provider = new BundleProvider();

        provider.ParametersCount.Should().Be(1);
        provider.GetAttributes(new BigInteger(2))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(898), "number", new BigInteger(950)),
                new("StartTime", new BigInteger(1690286212), "date"),
                new("FinishTime", new BigInteger(1690385286), "date"),
            });
    }
}