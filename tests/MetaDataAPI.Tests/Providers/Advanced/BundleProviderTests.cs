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
            .RespondWith(HttpMock.DealResponse);

        var provider = new BundleProvider(new BigInteger(0));

        provider.ParametersCount.Should().Be(1);
        provider.GetAttributes(new BigInteger(2))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number"),
                new("LeftAmount", new BigInteger(0), "number")
            });
    }
}