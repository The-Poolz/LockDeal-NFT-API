using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public abstract class ToDrawing
{
    public abstract Image Draw(Image drawOn);

    protected static Image Draw(Image drawOn, string text, PointF coordinates, float fontSize, FontStyle fontStyle = FontStyle.Regular)
    {
        var font = LoadFont(fontSize, fontStyle);
        var imageProcessor = new ImageProcessor(drawOn, font);
        return imageProcessor.DrawText(text, coordinates);
    }

    protected static Image DrawCurrencySymbol(Image drawOn, string currencySymbol, PointF coordinates, FontStyle fontStyle = FontStyle.Regular)
    {
        var font = LoadFont(16f, fontStyle);
        var imageProcessor = new ImageProcessor(drawOn, font);
        return imageProcessor.DrawCurrencySymbol(currencySymbol, coordinates);
    }

    protected static Font LoadFont(float fontSize, FontStyle fontStyle)
    {
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        return new ResourcesLoader().LoadFontFromEmbeddedResources(fontSize, fontStyle);
    }
}