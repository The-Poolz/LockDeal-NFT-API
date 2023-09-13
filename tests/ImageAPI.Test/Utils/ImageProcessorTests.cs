using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Xunit;
using SixLabors.ImageSharp.PixelFormats;
using FluentAssertions;

namespace ImageAPI.Test.Utils;

public class ImageProcessorTests
{
    ImageProcessor imageProcessor = new ImageProcessor();

    [Fact]
    public async Task GetBase64ImageAsync_ShouldReturnCorrectBase64String()
    {
        using  var imageStream = new MemoryStream();
        using var image = new Image<Rgba32>(100, 100);

        image.Mutate(x => x.BackgroundColor(Color.White));

        var imageBytes = imageStream.ToArray();
        var expectedResult = Convert.ToBase64String(imageBytes);

        var result = await imageProcessor.GetBase64ImageAsync();

        result.Should().NotBeNullOrEmpty();
        result.Should().NotBeEquivalentTo(expectedResult);
    }


    [Fact]
    public void DrawText_ShouldDrawCorrectTextOnImage()
    {
        var textOptions = imageProcessor.CreateTextOptions(50, 50, 100);;

        Action drawTextResult = () => imageProcessor.DrawText("Text for test", textOptions);
        
        drawTextResult.Should().NotThrow();
    }


    [Fact]
    public void CreateTextOptions_ShouldCreateCorrectTextOptions()
    {
        float x = 10;
        float y = 20;
        float wrappingLength = 200;

        var result = imageProcessor.CreateTextOptions(x, y, wrappingLength);

        Assert.NotNull(result);
        Assert.Equal(x, result.Origin.X);
        Assert.Equal(y, result.Origin.Y);
        Assert.Equal(4, result.TabWidth);
        Assert.Equal(wrappingLength, result.WrappingLength);
        Assert.Equal(HorizontalAlignment.Right, result.HorizontalAlignment);
    }
}
