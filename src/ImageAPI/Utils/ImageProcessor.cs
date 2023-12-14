using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Utils;

public class ImageProcessor
{
    private const string DefaultColorHex = "#FDFDFD";
    private readonly Font font;
    private readonly Image image;

    public ImageProcessor(Image image, Font font)
    {
        this.image = image;
        this.font = font;
    }

    public virtual Image DrawText(string text, PointF coordinates, IBrush? brush = null, IPen? pen = null)
    {
        brush ??= Brushes.Solid(Color.ParseHex(DefaultColorHex));
        pen ??= Pens.Solid(Color.ParseHex(DefaultColorHex), 2);
        var textOptions = CreateTextOptions(coordinates);

        image.Mutate(x => x.DrawText(textOptions, text, brush, pen));

        return image;
    }

    private TextOptions CreateTextOptions(PointF coordinates)
    {
        return new TextOptions(font)
        {
            Origin = coordinates,
            TabWidth = 4,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }
}