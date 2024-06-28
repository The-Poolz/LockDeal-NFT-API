using Moq;
using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Extensions;
using Nethereum.RPC.Eth.DTOs;
using poolz.finance.csharp.contracts.LockDealNFT;

namespace MetaDataAPI.Tests.Extensions;

public class LockDealNftServiceExtensionsTests
{
    public class IsPoolIdInSupplyRange
    {
        [Theory]
        [MemberData(nameof(TestData))]
        internal void ShouldReturnExpectedResult(BigInteger poolId, BigInteger totalSupply, bool expectedResult)
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
            yield return new object[] { 5, 10, true };
            yield return new object[] { 10, 10, false };
            yield return new object[] { 15, 10, false };
        }
    }
}