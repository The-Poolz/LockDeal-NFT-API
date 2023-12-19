using SixLabors.ImageSharp;

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
        return DrawCurrencySymbol(drawOn, $"{currencySymbol}", coordinates);
    }
}