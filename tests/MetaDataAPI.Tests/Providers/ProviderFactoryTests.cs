using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers;

public class ProviderFactoryTests : SetEnvironments
{
    [Fact]
    public void Create_ShouldCreateDealProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.DealAddress, new BigInteger(42));

        provider.Should().BeOfType<DealProvider>();
        provider.GetAttributes(new BigInteger(0))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number")
            });
    }

    [Fact]
    public void Create_ShouldCreateLockProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.LockAddress, new BigInteger(42));

        provider.Should().BeOfType<LockProvider>();
        provider.GetAttributes(new BigInteger(0), new BigInteger(1692090665))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number"),
                new("StartTime", new BigInteger(1692090665), "date")
            });
    }

    [Fact]
    public void Create_ShouldCreateTimedProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.TimedAddress, new BigInteger(42));

        provider.Should().BeOfType<TimedProvider>();
        provider.GetAttributes(new BigInteger(0), new BigInteger(1692090665), new BigInteger(1692090665), new BigInteger(100))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number", new BigInteger(100)),
                new("StartTime", new BigInteger(1692090665), "date"),
                new("FinishTime", new BigInteger(1692090665), "date"),
            });
    }

    [Fact]
    public void Create_ShouldCreateBundleProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.TimedResponse);

        var provider = ProviderFactory.Create(Environments.BundleAddress, new BigInteger(0));

        provider.Should().BeOfType<BundleProvider>();
        provider.GetAttributes(new BigInteger(2))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LastSubPoolId", new BigInteger(2), "number"),
                new("LeftAmount", new BigInteger(898), "number", new BigInteger(950)),
                new("StartTime", new BigInteger(1690286212), "date"),
                new("FinishTime", new BigInteger(1690385286), "date"),
            });
    }
}