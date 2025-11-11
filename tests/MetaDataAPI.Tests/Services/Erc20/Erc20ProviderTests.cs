using Moq;
using Xunit;
using Nethereum.Web3;
using FluentAssertions;
using MetaDataAPI.Extensions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using MetaDataAPI.Services.Http;
using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Services.ChainsInfo;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace MetaDataAPI.Tests.Services.Erc20;

public class Erc20ProviderTests
{
    public class GetErc20Token
    {
        [Fact]
        internal void ShouldReceiveExpectedErc20FromDynamoDb()
        {
            const long chainId = 97;
            const string baseRpcUrl = "https://rpc.example/";
            const string address = EthereumAddress.ZeroAddress;
            Environment.SetEnvironmentVariable(nameof(Env.BASE_URL_OF_RPC), baseRpcUrl);
            Environment.SetEnvironmentVariable(nameof(Env.MULTI_CALL_V3_ADDRESS), EthereumAddress.ZeroAddress);

            var cacheProvider = new Mock<IErc20CacheService>();
            var web3Factory = new Mock<IWeb3Factory>();
            var web3 = new Mock<IWeb3>();

            var cacheItem = new Erc20TokenDynamoDbEntry(
                new HashKey(chainId, address),
                new Erc20TokenData(address, "NAME", "SYMBOL", 8, 100)
            );

            web3Factory
                .Setup(x => x.Create(chainId.ToRpcUrl()))
                .Returns(web3.Object);

            cacheProvider
                .Setup(x => x.GetOrAddAsync(
                    It.IsAny<HashKey>(),
                    It.IsAny<Func<Task<IWeb3>>>(),
                    It.IsAny<Func<Task<EthereumAddress>>>()
                ))
                .ReturnsAsync(cacheItem);

            var provider = new Erc20Provider(cacheProvider.Object, web3Factory.Object);

            var result = provider.GetErc20Token(
                new ChainInfo(chainId, EthereumAddress.ZeroAddress),
                EthereumAddress.ZeroAddress
            );

            result.Should().BeEquivalentTo(new Erc20Token(cacheItem));
        }
    }
}