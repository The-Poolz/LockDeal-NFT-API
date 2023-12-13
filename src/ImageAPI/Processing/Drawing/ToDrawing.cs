using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public abstract class ToDrawing
{
    public const float AmountFontSize = 24f;
    public const float DateTimeFontSize = 14f;
    public const float TextFontSize = 14f;
    public const float ProviderNameFontSize = 32f;

    public abstract string Text { get; }
    public abstract Font Font { get; }
    public abstract PointF Coordinates { get; }

    public Image Draw(Image drawOn)
    {
        var imageProcessor = new ImageProcessor(drawOn, Font);
        var options = imageProcessor.CreateTextOptions(Coordinates);
        return imageProcessor.DrawText(Text, options);
    }
}