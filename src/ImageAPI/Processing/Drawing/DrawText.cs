using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawText : ToDrawing
{
    public override string Text { get; }
    public override Font Font { get; }
    public override PointF Coordinates { get; }

    public DrawText(string text, float x, float y)
    {
        Text = text;
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        Font = new ResourcesLoader().LoadFontFromEmbeddedResources(TextFontSize);
        Coordinates = new PointF(x, y);
    }
}