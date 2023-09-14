using Moq;
using Xunit;
using ImageAPI.Utils;
using SixLabors.Fonts;
using FluentAssertions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.Test.Utils;

public class ResourcesLoaderTests
{
    [Fact]
    public void LoadImageFromEmbeddedResources_ShouldLoadExpectedImage()
    {
        var resourcesLoader = new ResourcesLoader();
        var result = resourcesLoader.LoadImageFromEmbeddedResources();

        Assert.NotNull(result);
        Assert.IsType<Image<Rgba32>>(result);
    }

    [Fact]
    public void LoadImageFromEmbeddedResources_ShouldThrowException()
    {
        var resourcesLoader = new Mock<ResourcesLoader>();
        resourcesLoader
            .Setup(x => x.LoadImageFromEmbeddedResources())
            .CallBase();

        var testCode = () => resourcesLoader.Object.LoadImageFromEmbeddedResources();

        var exception = testCode.Should().Throw<FileNotFoundException>();
        exception.WithMessage($"Could not find the embedded resource '{ResourcesLoader.BackgroundResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.");
    }

    [Fact]
    public void LoadFontFromEmbeddedResources_ShouldLoadExpectedFont()
    {
        var resourcesLoader = new ResourcesLoader();
        var result = resourcesLoader.LoadFontFromEmbeddedResources();

        Assert.NotNull(result);
        Assert.IsType<Font>(result);
    }

    [Fact]
    public void LoadFontFromEmbeddedResources_ShouldThrowException()
    {
        var resourcesLoader = new Mock<ResourcesLoader>();
        resourcesLoader
            .Setup(x => x.LoadFontFromEmbeddedResources(It.IsAny<float>()))
            .CallBase();

        var testCode = () => resourcesLoader.Object.LoadFontFromEmbeddedResources();

        var exception = testCode.Should().Throw<FileNotFoundException>();
        exception.WithMessage($"Could not find the embedded resource '{ResourcesLoader.FontResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.");
    }
}
