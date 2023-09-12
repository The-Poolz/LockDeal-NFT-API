using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Utils;

public class ImageProcessor
{
    private readonly Image image;
    private readonly Font font;

    public ImageProcessor()
    {
        image = ResourcesLoader.LoadImageFromEmbeddedResources();
        font = ResourcesLoader.LoadFontFromEmbeddedResources();
    }

    public async Task<string> GetBase64ImageAsync()
    {
        using var outputStream = new MemoryStream();
        await image.SaveAsPngAsync(outputStream);
        var imageBytes = outputStream.ToArray();

        return Convert.ToBase64String(imageBytes);
    }

    public void DrawText(string text, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        brush ??= Brushes.Solid(Color.Black);
        pen ??= Pens.Solid(Color.DarkRed, 2);

        image.Mutate(x => x.DrawText(textOptions, text, brush, pen));
    }

    public TextOptions CreateTextOptions(float x, float y, float wrappingLength = 100)
    {
        return new TextOptions(font)
        {
            Origin = new PointF(x, y),
            TabWidth = 4,
            WrappingLength = wrappingLength,
            HorizontalAlignment = HorizontalAlignment.Right
        };
    }
}