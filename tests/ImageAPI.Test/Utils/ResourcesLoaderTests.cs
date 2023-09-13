using System.Reflection;
using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace ImageAPI.Tests;

public class ResourcesLoaderTests
{
    [Fact]
    public void LoadImageFromEmbeddedResources_ShouldLoadExpectedImage()
    {
        var result = ResourcesLoader.LoadImageFromEmbeddedResources();

        Assert.NotNull(result);
        Assert.IsType<Image<Rgba32>>(result);

        using var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ImageAPI.Resources.Sample-png-Image-for-Testing.png");

        if (imageStream != null )
        {
            var expectedResult = Image.Load<Rgba32>(imageStream);

            // Compare the dimensions of the loaded image
            Assert.Equal(expectedResult.Width, result.Width);
            Assert.Equal(expectedResult.Height, result.Height);
        }
    }

    [Fact]
    public void LoadFontFromEmbeddedResources_ShouldLoadExpectedFont()
    {
        float expectedSize = 24f;

        var result = ResourcesLoader.LoadFontFromEmbeddedResources();

        Assert.NotNull(result);
        Assert.IsType<Font>(result);

        using var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ImageAPI.Resources.LEMONMILK-Regular.otf");
        
        if (fontStream != null)
        {
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Add(fontStream);
            var expectedResult = new Font(fontFamily, expectedSize);

            // Compare font family and size
            Assert.Equal(expectedResult.Family.Name, result.Family.Name);
            Assert.Equal(expectedSize, result.Size);
        }
    }
}
