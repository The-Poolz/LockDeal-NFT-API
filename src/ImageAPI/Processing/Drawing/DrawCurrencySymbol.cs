using ImageAPI.Utils;
using SixLabors.ImageSharp;
using static ImageAPI.Processing.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawCurrencySymbol : ToDrawing
{
    private readonly object currencySymbol;
    private readonly PointF coordinates;

    public DrawCurrencySymbol(object currencySymbol, PointF coordinates)
    {
        this.currencySymbol = currencySymbol;
        this.coordinates = coordinates;
    }

    public override Image Draw(Image drawOn)
    {
        return DrawCurrencySymbol(drawOn, $"${currencySymbol}", coordinates);
        //return Draw(drawOn, $"${currencySymbol}", new PointF(ImageSize.Width - 554, ImageSize.Height - 270), TextFontSize);
    }
}