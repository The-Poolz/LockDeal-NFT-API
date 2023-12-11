using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawLeftAmount : ToDrawing
{
    public override string Text { get; }
    public override Font Font { get; }
    public override PointF Coordinates { get; }

    public DrawLeftAmount(object leftAmount)
    {
        Text = leftAmount.ToString()!;
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        Font = new ResourcesLoader().LoadFontFromEmbeddedResources(AttributeFontSize);
        Coordinates = new PointF(ImageSize.Width - 400, ImageSize.Height - 290);
    }
}