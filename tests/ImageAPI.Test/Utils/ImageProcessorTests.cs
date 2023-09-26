using Xunit;
using ImageAPI.Utils;
using SixLabors.Fonts;
using FluentAssertions;
using SixLabors.ImageSharp;

namespace ImageAPI.Test.Utils;

public class ImageProcessorTests
{
    private readonly ImageProcessor imageProcessor = new();

    [Fact]
    public void DrawText_ShouldDrawCorrectTextOnImage()
    {
        var textOptions = imageProcessor.CreateTextOptions(new PointF(50, 50));

        var drawTextResult = () => imageProcessor.DrawText("Text for test", textOptions);
        
        drawTextResult.Should().NotThrow();
    }

    [Fact]
    public void CreateTextOptions_ShouldCreateCorrectTextOptions()
    {
        const float x = 10;
        const float y = 20;

        var result = imageProcessor.CreateTextOptions(new PointF(x, y));

        Assert.NotNull(result);
        Assert.Equal(x, result.Origin.X);
        Assert.Equal(y, result.Origin.Y);
        Assert.Equal(4, result.TabWidth);
        Assert.Equal(HorizontalAlignment.Left, result.HorizontalAlignment);
    }
}
