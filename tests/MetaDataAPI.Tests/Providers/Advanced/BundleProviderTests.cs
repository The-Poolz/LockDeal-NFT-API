using Xunit;
using System.Numerics;
using AutoMapper;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Models.Types;
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
        var values = new BigInteger[] { new(2) };

        var provider = new BundleProvider(new BigInteger(0), 18, values);

        provider.ParametersCount.Should().Be(1);
        provider.Attributes
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount_1", 0m, DisplayType.Number),
                new("LeftAmount_2", 0m, DisplayType.Number)
            });
    }

    [Fact]
    public void GetDescription_ShouldExpectedDescription()
    {
        const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";
        var values = new BigInteger[] { 45 };
        var httpTest = new HttpTest();
        httpTest.SetupRpcCall(43, HttpMock.DealResponse);
        httpTest.SetupRpcCall(44, HttpMock.LockResponse);
        httpTest.SetupRpcCall(45, HttpMock.TimedResponse);
        var provider = new BundleProvider(42, 18, values);

        var result = provider.GetDescription(token);

        var expectedMessage =
            "This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:" +
            $"{Environment.NewLine}- 43: This NFT represents immediate access to 0 units of the specified asset 0x66134461c865f824d294d8ca0d9080cc1acd05f6." +
            $"{Environment.NewLine}- 44: This NFT securely locks 0 units of the asset 0x66134461c865f824d294d8ca0d9080cc1acd05f6. Access to these assets will commence on the designated start time of 1690286212." +
            $"{Environment.NewLine}- 45: This NFT governs a time-locked pool containing 0.000000000000000898 units of the asset 0x66134461c865f824d294d8ca0d9080cc1acd05f6. Withdrawals are permitted in a linear fashion beginning at 1690286212, culminating in full access at 1690385286.{Environment.NewLine}";
        result.Should()
            .BeEquivalentTo(expectedMessage);
    }
}