using Moq;
using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Services.ChainsInfo;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace MetaDataAPI.Tests.Services.Erc20;

public class Erc20ProviderTests
{
    public class Constructors
    {
        [Fact]
        internal void Default()
        {
            Environment.SetEnvironmentVariable("OVERRIDE_AWS_REGION", "us-east-1");
            var provider = new Erc20Provider();

            provider.Should().NotBeNull();
        }

        [Fact]
        internal void WithParameters()
        {
            var cacheProvider = new Erc20CacheService();

            var provider = new Erc20Provider(cacheProvider);

            provider.Should().NotBeNull();
        }
    }

    public class GetErc20Token
    {
        [Fact]
        internal void ShouldReceiveExpectedErc20FromDynamoDb()
        {
            Environment.SetEnvironmentVariable(nameof(Environments.BASE_URL_OF_RPC), "https://www.google.com");
            const int chainId = 97;
            const string address = EthereumAddress.ZeroAddress;

            var cacheProvider = new Mock<IErc20CacheService>();
            var cacheItem = new Erc20TokenDynamoDbEntry(
                new HashKey(chainId, address),
                new Erc20TokenData(address, "NAME", "SYMBOL", 8, 100)
            );
            cacheProvider.Setup(x => x.GetOrAddAsync(It.IsAny<HashKey>(), It.IsAny<Func<Task<string>>>(), It.IsAny<Func<Task<EthereumAddress>>>()))
                .ReturnsAsync(cacheItem);

            var provider = new Erc20Provider(cacheProvider.Object);

            var result = provider.GetErc20Token(
                new ChainInfo(chainId, EthereumAddress.ZeroAddress),
                EthereumAddress.ZeroAddress
            );

            result.Should().BeEquivalentTo(new Erc20Token(cacheItem));
        }
    }
}