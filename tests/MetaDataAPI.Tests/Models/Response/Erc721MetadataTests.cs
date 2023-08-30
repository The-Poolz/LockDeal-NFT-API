using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers;
using System.Numerics;
using Xunit;

namespace MetaDataAPI.Tests.Models.Response;

public class Erc721MetadataTests
{
    private const string owner = "0x57e0433551460e85dfc5a5ddaff4db199d0f960a";
    private const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";

    [Fact]
    public void GetDescription_ReceiveDealDescription()
    {
        // Arrange
        var poolId = new BigInteger(0);
        var provider = ProviderFactory.Create(ProviderName.Deal, poolId, 18);
        var parameters = new BigInteger[] { new(1000000000000000000) };
        var poolInfo = new BasePoolInfo(provider, poolId, owner, token, parameters);
        var erc721Metadata = new Erc721Metadata(poolInfo);

        // Act 
        var result = erc721Metadata.GetDescription(poolInfo);

        // Assert
        Assert.Equal(Erc721Metadata.DealDescription(1, token), result);
    }

    [Fact]
    public void GetDescription_ReceiveLockDescription()
    {
        // Arrange
        var poolId = new BigInteger(0);
        var provider = ProviderFactory.Create(ProviderName.Lock, poolId, 18);
        var parameters = new BigInteger[] { new(1000000000000000000) , new(1690286212) };
        var poolInfo = new BasePoolInfo(provider, poolId, owner, token, parameters);
        var erc721Metadata = new Erc721Metadata(poolInfo);

        // Act 
        var result = erc721Metadata.GetDescription(poolInfo);

        // Assert
        Assert.Equal(Erc721Metadata.LockDescription(1, token, 1690286212), result);
    }

    [Fact]
    public void GetDescription_ReceiveTimedDescription()
    {
        // Arrange
        var poolId = new BigInteger(0);
        var provider = ProviderFactory.Create(ProviderName.Timed, poolId, 18);
        var parameters = new BigInteger[] { new(1000000000000000000), new(1690286212), new(1690385286), new(1000000000000000000) };
        var poolInfo = new BasePoolInfo(provider, poolId, owner, token, parameters);
        var erc721Metadata = new Erc721Metadata(poolInfo);

        // Act 
        var result = erc721Metadata.GetDescription(poolInfo);

        // Assert
        Assert.Equal(Erc721Metadata.TimedDescription(1, token, 1690286212, 1690385286), result);
    }
}
