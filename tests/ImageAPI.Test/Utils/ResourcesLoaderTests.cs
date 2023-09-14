using Xunit;
using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.Test.Utils;

public class ResourcesLoaderTests
{
    [Fact]
    public void LoadImageFromEmbeddedResources_ShouldLoadExpectedImage()
    {
        var result = ResourcesLoader.LoadImageFromEmbeddedResources();

        Assert.NotNull(result);
        Assert.IsType<Image<Rgba32>>(result);
    }

    [Fact]
    public void LoadFontFromEmbeddedResources_ShouldLoadExpectedFont()
    {
        var result = ResourcesLoader.LoadFontFromEmbeddedResources();

        Assert.NotNull(result);
        Assert.IsType<Font>(result);
    }
}
