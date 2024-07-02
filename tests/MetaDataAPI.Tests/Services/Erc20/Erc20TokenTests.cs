using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using MetaDataAPI.Services.Erc20;

namespace MetaDataAPI.Tests.Services.Erc20;

public class Erc20TokenTests
{
    public new class ToString
    {
        [Fact]
        internal void ShouldExpectedStringThatRepresentToken()
        {
            var erc20Token = new Erc20Token(
                name: "TEST NAME",
                symbol: "TST",
                address: EthereumAddress.ZeroAddress, 
                decimals: 8
            );

            var result = erc20Token.ToString();

            result.Should().Be("TEST NAME (TST@0x00000...00000)");
        }
    }
}