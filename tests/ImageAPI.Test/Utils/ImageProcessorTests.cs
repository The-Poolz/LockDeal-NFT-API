using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Xunit;
using SixLabors.ImageSharp.Processing.Processors;
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
        // Arrange
        string text = "abc";
        var textOptions = imageProcessor.CreateTextOptions(50, 50, 100);
        var brush = Brushes.Solid(Color.Blue);
        var pen = Pens.Solid(Color.Green, 2);

        imageProcessor.DrawText(text, textOptions, brush, pen);

        /*var base64Image = imageProcessor.GetBase64ImageAsync().Result;
        using var decodedImageStream = new MemoryStream(Convert.FromBase64String(base64Image));
        using var decodedImage = Image.Load<Rgba32>(decodedImageStream);

        decodedImage.Should().NotBeNull();

        var pixelColorAtX50Y50 = decodedImage[50, 50];
        var pixelColorAtX51Y51 = decodedImage[51, 51];
        var pixelColorAtX52Y52 = decodedImage[53, 53];*/
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
