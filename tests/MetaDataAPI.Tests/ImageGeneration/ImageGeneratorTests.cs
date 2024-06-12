using Moq;
using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.ImageGeneration;
using MetaDataAPI.ImageGeneration.Services;
using MetaDataAPI.ImageGeneration.UrlifyModels.Simple;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Tests.ImageGeneration;

public class ImageGeneratorTests
{
    public class Constructor : SetEnvironments
    {
        [Fact]
        internal void Default()
        {
            var imageGenerator = new ImageGenerator();

            imageGenerator.Should().NotBeNull();
        }
    }

    public class Generate : SetEnvironments
    {
        private readonly Mock<IImageRenderer> renderer = new();
        private readonly Mock<IUrlShortener> shortener = new();

        [Fact]
        internal void WhenImageUrlGeneratedSuccessfully()
        {
            var dealUrlifyModel = new DealUrlifyModel(new BasePoolInfo
            {
                Name = "Provider",
                PoolId = 1234,
                Token = "0x32a23a97daEd1F41fA6dFE72Cc33aD5bCBdf17E1",
                Owner = "0x000000000000000000000000000000000000dEaD",
                VaultId = 2,
                Provider = "0x70B0F2fd774376063faCC9178307cF1E18Ea3aF0",
                Params = new List<BigInteger> { 10000000000000000000 }
            });
            var url = "https://www.google.com";
            var description = "description";

            renderer.Setup(x => x.RenderImage(It.IsAny<string>()))
                .Returns(url);
            shortener.Setup(x => x.Shorten(url, description))
                .Returns(url);

            var imageGenerator = new ImageGenerator(renderer.Object, shortener.Object);

            var result = imageGenerator.Generate(dealUrlifyModel, description);

            result.Should().Be(url);
        }
    }
}