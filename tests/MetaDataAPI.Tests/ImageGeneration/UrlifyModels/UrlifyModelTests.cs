using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Extension;
using MetaDataAPI.ImageGeneration.UrlifyModels;
using MetaDataAPI.ImageGeneration.UrlifyModels.Simple;
using MetaDataAPI.ImageGeneration.UrlifyModels.Advanced;
using MetaDataAPI.ImageGeneration.UrlifyModels.DelayVault;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Tests.ImageGeneration.UrlifyModels;

public class UrlifyModelTests
{
    public class BuildUrl : SetEnvironments
    {
        [Theory]
        [MemberData(nameof(UrlifyModelData))]
        public void UrlifyModel_WhenExpectedUrlGenerated(Type modelType, string[] paramsArray, string expectedUrl)
        {
            var basePoolInfo = new BasePoolInfo
            {
                Name = "Provider",
                PoolId = 1234,
                Token = "0x32a23a97daEd1F41fA6dFE72Cc33aD5bCBdf17E1",
                Owner = "0x000000000000000000000000000000000000dEaD",
                VaultId = 2,
                Provider = "0x70B0F2fd774376063faCC9178307cF1E18Ea3aF0",
                Params = paramsArray.Select(BigInteger.Parse).ToList()
            };
            var poolInfo = new PoolInfo(basePoolInfo);
            var nftHtml = (BaseUrlifyModel)Activator.CreateInstance(modelType, poolInfo)!;

            var result = nftHtml.BuildUrl();

            result.ToString().Should().Be(expectedUrl);
        }

        public static IEnumerable<object[]> UrlifyModelData()
        {
            yield return new object[]
            {
                typeof(DealUrlifyModel),
                new[] { "10000000000000000000000" },
                "https://test.poolz.finance/nft.html?name=Provider&id=1234&tA=%24TestTokenC%7CLeft%20Amount%7C10000"
            };
            yield return new object[]
            {
                typeof(LockDealUrlifyModel),
                new[] { "10000000000000000000000", "1708089781" },
                "https://test.poolz.finance/nft.html?Start%20Time=02%2F16%2F2024%2013%3A23%3A01&name=Provider&id=1234&tA=%24TestTokenC%7CLeft%20Amount%7C10000"
            };
            yield return new object[]
            {
                typeof(TimedDealUrlifyModel),
                new[] { "10000000000000000000000", "1708089781", "1708989781" },
                "https://test.poolz.finance/nft.html?Finish%20Time=02%2F26%2F2024%2023%3A23%3A01&Start%20Time=02%2F16%2F2024%2013%3A23%3A01&name=Provider&id=1234&tA=%24TestTokenC%7CLeft%20Amount%7C10000"
            };
            yield return new object[]
            {
                typeof(DelayVaultUrlifyModel),
                new[] { "10000000000000000000000" },
                "https://test.poolz.finance/nft.html?name=Provider&id=1234&tA=%24TestTokenC%7CLeft%20Amount%7C10000"
            };
            yield return new object[]
            {
                typeof(RefundUrlifyModel),
                new[] { "10000000000000000000000" },
                "https://test.poolz.finance/nft.html?name=Provider&id=1234&tA=%24TestTokenC%7CLeft%20Amount%7C10000"
            };
            yield return new object[]
            {
                typeof(CollateralUrlifyModel),
                new[] { "10000000000000000000000" },
                "https://test.poolz.finance/nft.html?name=Provider&id=1234&tA=%24TestTokenC%7CLeft%20Amount%7C10000"
            };
        }
    }
}