using ImageAPI.Utils;
using SixLabors.Fonts;
using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;

namespace ImageAPI.Processing.Drawing;

public class DrawFinishTime : ToDrawing
{
    public override string Text { get; }
    public override Font Font { get; }
    public override PointF Coordinates { get; }

    public DrawFinishTime(object finishTime)
    {
        Text = TimeUtils.FromUnixTimestamp((long)finishTime).ToString(CultureInfo.InvariantCulture);
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        Font = new ResourcesLoader().LoadFontFromEmbeddedResources(AttributeFontSize);
        Coordinates = new PointF(ImageSize.Width - 730, ImageSize.Height - 290);
    }
}