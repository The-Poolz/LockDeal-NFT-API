using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawProviderName : ToDrawing
{
    public override string Text { get; }
    public override Font Font { get; }
    public override PointF Coordinates { get; }

    public DrawProviderName(string providerName)
    {
        Text = providerName;
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        Font = new ResourcesLoader().LoadFontFromEmbeddedResources(ProviderNameFontSize);
        Coordinates = new PointF(ImageSize.Width - 400, 50);
    }
}