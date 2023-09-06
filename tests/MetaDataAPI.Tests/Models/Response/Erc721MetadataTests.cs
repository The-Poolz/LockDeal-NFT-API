using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers;
using System.Numerics;
using Xunit;

namespace MetaDataAPI.Tests.Models.Response
{
    public class Erc721MetadataTests
    {
        private const string owner = "0x57e0433551460e85dfc5a5ddaff4db199d0f960a";
        private const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";

        private static BasePoolInfo GetBasePoolInfo(ProviderName providerName, BigInteger[] parameters)
        {
            var poolId = new BigInteger(0);
            var provider = ProviderFactory.Create(providerName, poolId, 18, parameters);
            return new BasePoolInfo(provider, poolId, owner, token);
        }

        [Fact]
        public void GetDescription_ReceiveDealDescription()
        {
            // Arrange
            var parameters = new BigInteger[] { new(1000000000000000000) };
            var poolInfo = GetBasePoolInfo(ProviderName.Deal, parameters);
            var erc721Metadata = new Erc721Metadata(poolInfo);

            // Act 
            var result = erc721Metadata.Description;

            // Assert
            Assert.Equal(poolInfo.Provider.GetDescription(token), result);
        }

        [Fact]
        public void GetDescription_ReceiveLockDescription()
        {
            // Arrange
            var parameters = new BigInteger[] { new(1000000000000000000), new(1690286212) };
            var poolInfo = GetBasePoolInfo(ProviderName.Lock, parameters);
            var erc721Metadata = new Erc721Metadata(poolInfo);

            // Act 
            var result = erc721Metadata.Description;

            // Assert
            Assert.Equal(poolInfo.Provider.GetDescription(token), result);
        }

        [Fact]
        public void GetDescription_ReceiveTimedDescription()
        {
            // Arrange
            var parameters = new BigInteger[] { new(1000000000000000000), new(1690286212), new(1690385286), new(1000000000000000000) };
            var poolInfo = GetBasePoolInfo(ProviderName.Timed, parameters);
            var erc721Metadata = new Erc721Metadata(poolInfo);

            // Act 
            var result = erc721Metadata.Description;

            // Assert
            Assert.Equal(poolInfo.Provider.GetDescription(token), result);
        }
    }
}