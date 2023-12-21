using SixLabors.ImageSharp;
using ImageAPI.Processing.Drawing.Options;

namespace ImageAPI.Processing.Drawing;

public abstract class ToDrawing
{
    public abstract void Draw(Image drawOn);

    protected static void Draw(Image drawOn, DrawOptions options)
    {
        drawOn.DrawText(options);
    }

    protected static void DrawCurrencySymbol(Image drawOn, DrawCurrencyOptions options)
    {
        drawOn.DrawCurrencySymbol(options);
    }
}