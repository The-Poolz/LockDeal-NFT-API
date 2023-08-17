using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers;

public class ProviderFactoryTests : SetEnvironments
{
    private readonly BigInteger poolId = new(0);

    [Fact]
    public void Create_ShouldCreateDealProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.DealAddress, poolId);

        provider.Should().BeOfType<DealProvider>();
        provider.GetAttributes(new BigInteger(0))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", 0m, DisplayType.Number)
            });
    }

    [Fact]
    public void Create_ShouldCreateLockProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.LockAddress, poolId);

        provider.Should().BeOfType<LockProvider>();
        provider.GetAttributes(new BigInteger(0), new BigInteger(1692090665))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", 0m, DisplayType.Number),
                new("StartTime", new BigInteger(1692090665), DisplayType.Date)
            });
    }

    [Fact]
    public void Create_ShouldCreateTimedProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.TimedAddress, poolId);

        provider.Should().BeOfType<TimedProvider>();
        provider.GetAttributes(new BigInteger(0), new BigInteger(1692090665), new BigInteger(1692090665), new BigInteger(100))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", 0m, DisplayType.Number, 0.0000000000000001m),
                new("StartTime", new BigInteger(1692090665), DisplayType.Date),
                new("FinishTime", new BigInteger(1692090665), DisplayType.Date),
            });
    }

    [Fact]
    public void Create_ShouldCreateBundleProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.DealResponse);

        var provider = ProviderFactory.Create(Environments.BundleAddress, poolId);

        provider.Should().BeOfType<BundleProvider>();
        provider.GetAttributes(new BigInteger(2))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount_1", 0m, DisplayType.Number),
                new("LeftAmount_2", 0m, DisplayType.Number)
            });
    }
}