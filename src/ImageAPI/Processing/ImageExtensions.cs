using SixLabors.Fonts;
using ImageAPI.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public static class ImageExtensions
{
    public static void DrawText(this Image drawOn, string text, PointF location, float penWidth, Color color = default)
    {
        color = color == default ? ColorPalette.White : color;
        var textOptions = new RichTextOptions(font)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Origin = new PointF(location.X - penWidth, location.Y - penWidth),
        };

        drawOn.Mutate(x => x.DrawText(textOptions, text, Brushes.Solid(color), Pens.Solid(color, penWidth)));
    }
}