using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public static class ImageExtensions
{
    public static Image DrawText(this Image image, string text, PointF location, float penWidth, Color color = default)
    {
        color = color == default ? white : color;
        var textOptions = new RichTextOptions(font)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Origin = new PointF(location.X - penWidth, location.Y - penWidth),
        };

        image.Mutate(x => x.DrawText(textOptions, text, Brushes.Solid(color), Pens.Solid(color, penWidth)));

        return image;
    }
}