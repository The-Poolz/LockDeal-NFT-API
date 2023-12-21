using ImageAPI.Processing.Drawing.Options;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawCurrency : ToDrawing
{
    private readonly object currencySymbol;
    private readonly PointF coordinates;

    public DrawCurrency(object currencySymbol, PointF coordinates)
    {
        this.currencySymbol = currencySymbol;
        this.coordinates = coordinates;
    }

    public override void Draw(Image drawOn)
    {
        DrawCurrencySymbol(drawOn, new DrawCurrencyOptions($"{currencySymbol}", coordinates));
    }
}