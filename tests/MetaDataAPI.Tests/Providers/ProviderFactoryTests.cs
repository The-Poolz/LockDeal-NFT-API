using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Storage;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers;

public class ProviderFactoryTests : SetEnvironments
{
    [Fact]
    public void Create_ShouldCreateBundleProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.DealResponse);
        var values = new BigInteger[] { new(2) };

        var provider = ProviderFactory.Create(Environments.BundleAddress, new BigInteger(0), 18, values);

        provider.Should().BeOfType<BundleProvider>();
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount_1", 0m, DisplayType.Number),
                new("LeftAmount_2", 0m, DisplayType.Number)
            });
    }

    [Fact]
    public void Create_ShouldCreateLockProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.LockMetadata);
        var values = new BigInteger[] { 0, 1690286212 };

        var provider = ProviderFactory.Create(Environments.LockAddress, new BigInteger(1), 18, values);

        provider.Should().BeOfType<LockProvider>();
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", 0m, DisplayType.Number),
                new("StartTime", new BigInteger(1690286212), DisplayType.Date)
            });
    }

    [Fact]
    public void Create_ShouldCreateRefundProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.SetupRpcCall(11, HttpMock.TimedResponse);
        httpTest.SetupRpcCall(12, HttpMock.CollateralResponse);

        var values = new BigInteger[] { 12, 100000000000000000 };

        var provider = ProviderFactory.Create(Environments.RefundAddress, new BigInteger(10), 18, values);

        provider.Should().BeOfType<RefundProvider>();
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("Rate", 0.1m, DisplayType.Number),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount", 0.000000000000000898m, DisplayType.Number, 0.00000000000000095m),
                new("StartTime", new BigInteger(1690286212), DisplayType.Date),
                new("FinishTime", new BigInteger(1690385286), DisplayType.Date)
            });
    }

    [Fact]
    public void Create_ShouldCreateCollateralProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.SetupRpcCall(13, HttpMock.DealResponse);
        httpTest.SetupRpcCall(14, HttpMock.DealResponse);
        httpTest.SetupRpcCall(15, HttpMock.DealResponse);

        var values = new BigInteger[] { 1000, 1690385286 };

        var provider = ProviderFactory.Create(Environments.CollateralAddress, new BigInteger(12), 18, values);

        provider.Should().BeOfType<CollateralProvider>();
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", 0.000000000000001M, DisplayType.Number),
                new("FinishTime", new BigInteger(1690385286), DisplayType.Date),
                new("MainCoin", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("Token", "0x66134461c865f824d294d8ca0d9080cc1acd05f6"),
                new("LeftAmount_13", 0M, DisplayType.Number),
                new("LeftAmount_14", 0M, DisplayType.Number),
                new("LeftAmount_15", 0M, DisplayType.Number),
            });
    }
}