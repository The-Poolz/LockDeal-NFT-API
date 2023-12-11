using ImageAPI.Utils;
using SixLabors.Fonts;
using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;

namespace ImageAPI.Processing.Drawing;

public class DrawStartTime : ToDrawing
{
    public override string Text { get; }
    public override Font Font { get; }
    public override PointF Coordinates { get; }

    public DrawStartTime(object startTime)
    {
        Text = TimeUtils.FromUnixTimestamp((long)startTime).ToString(CultureInfo.InvariantCulture);
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        Font = new ResourcesLoader().LoadFontFromEmbeddedResources(AttributeFontSize);
        Coordinates = new PointF(ImageSize.Width - 1030, ImageSize.Height - 290);
    }
}