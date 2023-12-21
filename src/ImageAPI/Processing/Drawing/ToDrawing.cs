using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using static ImageAPI.Processing.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public abstract class ToDrawing
{
    public abstract Image Draw(Image drawOn);

    protected static Image Draw(Image drawOn, ToDrawParameters parameters)
    {
        var font = LoadFont(parameters.FontSize, parameters.FontStyle);
        var imageProcessor = new ImageProcessor(drawOn, font);
        return imageProcessor.DrawText(parameters.Text, parameters.Location, parameters.PenWidth);
    }

    protected static Image DrawCurrencySymbol(Image drawOn, string currencySymbol, PointF coordinates, FontStyle fontStyle = FontStyle.Regular)
    {
        var font = LoadFont(CurrencySymbol.FontSize, fontStyle);
        var imageProcessor = new ImageProcessor(drawOn, font);
        return imageProcessor.DrawCurrencySymbol(currencySymbol, coordinates);
    }

    protected static Font LoadFont(float fontSize, FontStyle fontStyle)
    {
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        return new ResourcesLoader().LoadFontFromEmbeddedResources(fontSize, fontStyle);
    }
}