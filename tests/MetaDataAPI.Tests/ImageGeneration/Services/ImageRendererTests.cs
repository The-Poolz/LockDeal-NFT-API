using Xunit;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.ImageGeneration.Services;

namespace MetaDataAPI.Tests.ImageGeneration.Services;

public class ImageRendererTests
{
    public class RenderImage : SetEnvironments
    {
        private readonly IImageRenderer renderer = new ImageRenderer();

        [Fact]
        internal void WhenSuccessRendered()
        {
            var url = "https://www.google.com";

            var result = renderer.RenderImage(url);

            result.Should().Be("https://api3.poolz.finance/twitterbot.png?url=https://www.google.com&selector=.blockmodal");
        }
    }
}