using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Extension;
using MetaDataAPI.ImageGeneration.UrlifyModels;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Tests.ImageGeneration.UrlifyModels;

public class BaseUrlifyModelTests
{
    public class BuildUrl : SetEnvironments
    {
        [Fact]
        internal void WhenExpectedUrlGenerated()
        {
            var poolInfo = new PoolInfo(new BasePoolInfo
            {
                Name = "DealProvider",
                PoolId = 1234,
                Token = "0x32a23a97daEd1F41fA6dFE72Cc33aD5bCBdf17E1",
                Owner = "0x000000000000000000000000000000000000dEaD",
                VaultId = 2,
                Provider = "0x70B0F2fd774376063faCC9178307cF1E18Ea3aF0",
                Params = new List<BigInteger> { BigInteger.Parse("10000000000000000000000") }
            });
            var nftHtml = new BaseUrlifyModel(poolInfo);

            var result = nftHtml.BuildUrl();

            result.ToString().Should().Be("https://test.poolz.finance/nft.html?name=DealProvider&id=1234&tA=$TestTokenC|Left Amount|10000");
        }
    }
}