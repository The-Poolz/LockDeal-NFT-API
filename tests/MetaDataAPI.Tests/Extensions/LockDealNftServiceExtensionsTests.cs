using Moq;
using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Extensions;
using Nethereum.RPC.Eth.DTOs;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Tests.Extensions;

public class LockDealNftServiceExtensionsTests
{
    public class IsPoolIdInSupplyRange
    {
        [Theory]
        [MemberData(nameof(TestData))]
        internal void ShouldReturnExpectedResult(long poolId, BigInteger totalSupply, bool expectedResult)
        {
            var lockDealNft = new Mock<ILockDealNFTService>();
            lockDealNft
                .Setup(x => x.TotalSupplyQueryAsync(It.IsAny<BlockParameter>()))
                .ReturnsAsync(totalSupply);

            var result = lockDealNft.Object.IsPoolIdInSupplyRange(poolId);

            result.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { 5, new BigInteger(10), true };
            yield return new object[] { 10, new BigInteger(10), false };
            yield return new object[] { 15, new BigInteger(10), false };
        }
    }

    public class FetchPoolInfo
    {
        private readonly Mock<ILockDealNFTService> _lockDealNft = new();

        [Theory]
        [MemberData(nameof(TestData))]
        internal void FromLockDealNFTService_ShouldReturnExpectedPoolInfo(long poolId, GetFullDataOutputDTO fullData, BasePoolInfo[] expectedPoolInfo)
        {
            _lockDealNft
                .Setup(x => x.GetFullDataQueryAsync(poolId, It.IsAny<BlockParameter>()))
                .ReturnsAsync(fullData);

            var result = _lockDealNft.Object.FetchPoolInfo(poolId);

            result.Should().BeEquivalentTo(expectedPoolInfo);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        internal void FromServiceProvider_ShouldReturnExpectedPoolInfo(long poolId, GetFullDataOutputDTO fullData, BasePoolInfo[] expectedPoolInfo)
        {
            _lockDealNft
                .Setup(x => x.GetFullDataQueryAsync(poolId, It.IsAny<BlockParameter>()))
                .ReturnsAsync(fullData);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(ILockDealNFTService)))
                .Returns(_lockDealNft.Object);

            var result = serviceProvider.Object.FetchPoolInfo(poolId);

            result.Should().BeEquivalentTo(expectedPoolInfo);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new BigInteger(1),
                new GetFullDataOutputDTO
                {
                    PoolInfo = new List<BasePoolInfo> { new() { PoolId = 1 } }
                },
                new BasePoolInfo[] { new() { PoolId = 1 } }
            };
        }
    }
}