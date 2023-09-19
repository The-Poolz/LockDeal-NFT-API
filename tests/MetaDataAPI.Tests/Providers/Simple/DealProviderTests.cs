using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers;
using Moq;
using MetaDataAPI.Tests.Helpers;

namespace MetaDataAPI.Tests.Providers.Simple;

public class DealProviderTests
{
    string rawMetaData = "0x00000000000000000000000000000000000000000000000000000000000000200000000000000000000000006b31be09cf4e2da92f130b1056717fea06176ced000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000006063fba0fbd645d648c129854cce45a70dd8969100000000000000000000000043d81a2cf49238484d6960de1df9d430c81cdffc00000000000000000000000000000000000000000000000000000000000000c00000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000002b5e3af16b1880000";

    [Fact]
    public void ProviderName_IsCorrect()
    {
        var rpcCaller = new MockRpcCaller();
        var providerFactory = new ProviderFactory(rpcCaller);

        var basePoolInfo = new BasePoolInfo(rawMetaData, providerFactory);

        var dealProvider = new DealProvider(basePoolInfo);

        Assert.Equal("DealProvider", dealProvider.ProviderName);
    }

    [Fact]
    public void Description_ShouldBeExpectedDescription()
    {
        var values = new BigInteger[] { 50 };

        var rpcCaller = new MockRpcCaller();
        var providerFactory = new ProviderFactory(rpcCaller);
        var basePoolInfo = new BasePoolInfo(rawMetaData, providerFactory);

        var provider = new DealProvider(basePoolInfo);

        var token = provider.PoolInfo.Token.ToString();
        var expectedResult = $"This NFT represents immediate access to {values[0]} units of the specified asset {token}.";
        var result = provider.Description;

        expectedResult.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void ProviderAttributes_ShouldReturnExpectedAttributes()
    {
        const decimal leftAmount = 50;
        var rpcCaller = new MockRpcCaller();
        var providerFactory = new ProviderFactory(rpcCaller);
        var basePoolInfo = new BasePoolInfo(rawMetaData, providerFactory);

        var provider = new DealProvider(basePoolInfo);
        var attributes = provider.ProviderAttributes;
        var expectedAttributes = new Erc721Attribute[]
        {
            new Erc721Attribute("LeftAmount", leftAmount, DisplayType.Number)
        };

        attributes.Should().BeEquivalentTo(expectedAttributes);
    }

    [Fact]
    public void Constructor_IsCorrect()
    {
        var rpcCaller = new MockRpcCaller();    
        var providerFactory = new ProviderFactory(rpcCaller);
        var basePoolInfo = new BasePoolInfo(rawMetaData, providerFactory);

        var provider = new DealProvider(basePoolInfo);
        decimal leftAmount = provider.LeftAmount;

        leftAmount.Should().Be(50);
    }
}

