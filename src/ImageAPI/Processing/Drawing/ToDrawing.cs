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

    private readonly Font font;
    private readonly string text;
    private readonly PointF coordinates;

    protected ToDrawing(string text, PointF coordinates, float fontSize, FontStyle fontStyle = FontStyle.Regular)
    {
        this.text = text;
        this.coordinates = coordinates;
        font = LoadFont(fontSize, fontStyle);
    }

    public Image Draw(Image drawOn)
    {
        var imageProcessor = new ImageProcessor(drawOn, font);
        var options = imageProcessor.CreateTextOptions(coordinates);
        return imageProcessor.DrawText(text, options);
    }

    private static Font LoadFont(float fontSize, FontStyle fontStyle)
    {
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        return new ResourcesLoader().LoadFontFromEmbeddedResources(fontSize, fontStyle);
    }
}